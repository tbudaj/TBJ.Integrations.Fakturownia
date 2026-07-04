using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Configuration;
using TBJ.Integrations.Fakturownia.Exceptions;
using Microsoft.Extensions.Logging;

namespace TBJ.Integrations.Fakturownia.Internal;

/// <summary>
/// Wewnętrzny klient HTTP do komunikacji z Fakturownia API.
/// Obsługuje logikę fallback credentials (appsettings → per-request auth).
/// </summary>
internal sealed class FakturowniaHttpClient
{
    private readonly HttpClient _http;
    private readonly FakturowniaOptions _options;
    private readonly ILogger<FakturowniaHttpClient> _logger;

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    public FakturowniaHttpClient(
        HttpClient http,
        FakturowniaOptions options,
        ILogger<FakturowniaHttpClient> logger)
    {
        _http = http;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Pobiera efektywne credentials — per-request lub z appsettings (fallback).
    /// </summary>
    /// <exception cref="FakturowniaException">Gdy brak credentials zarówno per-request jak i w konfiguracji.</exception>
    private (string apiToken, string domain) ResolveCredentials(FakturowniaAuthInfo? auth)
    {
        if (auth is not null)
            return (auth.ApiToken, auth.Domain);

        if (!string.IsNullOrWhiteSpace(_options.ApiToken) && !string.IsNullOrWhiteSpace(_options.Domain))
            return (_options.ApiToken!, _options.Domain!);

        throw FakturowniaException.MissingCredentials();
    }

    /// <summary>
    /// Buduje bazowy URL dla danej domeny konta Fakturownia.
    /// </summary>
    private static string BuildBaseUrl(string domain) =>
        $"https://{domain}.fakturownia.pl";

    /// <summary>
    /// Wykonuje żądanie GET i deserializuje odpowiedź jako <typeparamref name="T"/>.
    /// Parametr <c>api_token</c> dołączany jest do query string.
    /// </summary>
    public async Task<T> GetAsync<T>(
        string endpoint,
        FakturowniaAuthInfo? auth,
        IDictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = BuildUrl(domain, endpoint, apiToken, queryParams);

        _logger.LogDebug("Fakturownia GET {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.GetAsync(url, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);

        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    /// <summary>
    /// Wykonuje żądanie GET i zwraca surowe bajty odpowiedzi (np. PDF).
    /// </summary>
    public async Task<byte[]> GetBytesAsync(
        string endpoint,
        FakturowniaAuthInfo? auth,
        IDictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = BuildUrl(domain, endpoint, apiToken, queryParams);

        _logger.LogDebug("Fakturownia GET (bytes) {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.GetAsync(url, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);

        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    /// <summary>
    /// Wykonuje żądanie POST z body JSON i deserializuje odpowiedź jako <typeparamref name="T"/>.
    /// Parametr <c>api_token</c> wstrzykiwany jest do body JSON (zgodnie z API Fakturownia).
    /// </summary>
    public async Task<T> PostAsync<T>(
        string endpoint,
        FakturowniaAuthInfo? auth,
        object body,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = $"{BuildBaseUrl(domain)}/{endpoint}";

        var wrappedBody = WrapWithToken(body, apiToken);
        _logger.LogDebug("Fakturownia POST {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync(url, wrappedBody, JsonOptions, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);

        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    /// <summary>
    /// Wykonuje żądanie POST bez zwracanej treści (np. wyślij e-mail).
    /// </summary>
    public async Task PostAsync(
        string endpoint,
        FakturowniaAuthInfo? auth,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = BuildUrl(domain, endpoint, apiToken);

        _logger.LogDebug("Fakturownia POST (no body) {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsync(url, null, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);
    }

    /// <summary>
    /// Wykonuje żądanie PUT z body JSON i deserializuje odpowiedź jako <typeparamref name="T"/>.
    /// </summary>
    public async Task<T> PutAsync<T>(
        string endpoint,
        FakturowniaAuthInfo? auth,
        object body,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = $"{BuildBaseUrl(domain)}/{endpoint}";

        var wrappedBody = WrapWithToken(body, apiToken);
        _logger.LogDebug("Fakturownia PUT {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.PutAsJsonAsync(url, wrappedBody, JsonOptions, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);

        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    /// <summary>
    /// Wykonuje żądanie DELETE.
    /// </summary>
    public async Task DeleteAsync(
        string endpoint,
        FakturowniaAuthInfo? auth,
        CancellationToken ct = default)
    {
        var (apiToken, domain) = ResolveCredentials(auth);
        var url = BuildUrl(domain, endpoint, apiToken);

        _logger.LogDebug("Fakturownia DELETE {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.DeleteAsync(url, ct);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || !ct.IsCancellationRequested)
        {
            throw new FakturowniaException("Przekroczono limit czasu żądania do Fakturownia API.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FakturowniaException($"Błąd komunikacji z Fakturownia API: {ex.Message}", ex);
        }

        await EnsureSuccessAsync(response, ct);
    }

    private static string BuildUrl(
        string domain,
        string endpoint,
        string apiToken,
        IDictionary<string, string?>? extraParams = null)
    {
        var builder = new UriBuilder($"{BuildBaseUrl(domain)}/{endpoint}");
        var query = System.Web.HttpUtility.ParseQueryString(builder.Query);
        query["api_token"] = apiToken;

        if (extraParams is not null)
            foreach (var (key, value) in extraParams)
                if (value is not null)
                    query[key] = value;

        builder.Query = query.ToString();
        return builder.ToString();
    }

    /// <summary>
    /// Opakowuje body w obiekt z polem <c>api_token</c> wymaganym przez Fakturownia API dla POST/PUT.
    /// </summary>
    private static object WrapWithToken(object body, string apiToken)
    {
        // Serializuj body do JsonDocument, dodaj api_token na poziomie głównym
        var json = JsonSerializer.SerializeToDocument(body, JsonOptions);
        var dict = new Dictionary<string, object?> { ["api_token"] = apiToken };

        foreach (var prop in json.RootElement.EnumerateObject())
            dict[prop.Name] = prop.Value.Clone();

        return dict;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
            return;

        var statusCode = (int)response.StatusCode;
        string? body = null;

        try
        {
            body = await response.Content.ReadAsStringAsync(ct);
        }
        catch
        {
            // ignoruj błąd odczytu treści
        }

        throw new FakturowniaException(
            $"Fakturownia API zwróciło błąd HTTP {statusCode}.",
            statusCode,
            body);
    }
}
