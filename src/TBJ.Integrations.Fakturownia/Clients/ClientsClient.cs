using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Clients;

namespace TBJ.Integrations.Fakturownia.Clients;

/// <summary>
/// Implementacja klienta kontrahentów Fakturownia API.
/// </summary>
internal sealed class ClientsClient : IClientsClient
{
    private readonly FakturowniaHttpClient _http;

    public ClientsClient(FakturowniaHttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Client>> GetClientsAsync(
        string? name = null,
        string? taxNo = null,
        int page = 1,
        int perPage = 25,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["page"] = page.ToString(),
            ["per_page"] = perPage.ToString(),
        };

        if (name is not null) queryParams["name"] = name;
        if (taxNo is not null) queryParams["tax_no"] = taxNo;

        return _http.GetAsync<IReadOnlyList<Client>>("clients.json", auth, queryParams, ct);
    }

    /// <inheritdoc/>
    public Task<Client> GetClientAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<Client>($"clients/{id}.json", auth, ct: ct);

    /// <inheritdoc/>
    public Task<Client> CreateClientAsync(
        CreateClientRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PostAsync<Client>("clients.json", auth, new { client = request }, ct);

    /// <inheritdoc/>
    public Task<Client> UpdateClientAsync(
        long id,
        UpdateClientRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PutAsync<Client>($"clients/{id}.json", auth, new { client = request }, ct);

    /// <inheritdoc/>
    public Task DeleteClientAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.DeleteAsync($"clients/{id}.json", auth, ct);
}
