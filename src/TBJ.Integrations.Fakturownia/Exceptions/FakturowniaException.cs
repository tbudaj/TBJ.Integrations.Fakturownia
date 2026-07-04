namespace TBJ.Integrations.Fakturownia.Exceptions;

/// <summary>
/// Wyjątek zgłaszany przy błędach komunikacji z Fakturownia API.
/// </summary>
public class FakturowniaException : Exception
{
    /// <summary>
    /// Kod statusu HTTP zwrócony przez API (jeśli dostępny).
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Treść odpowiedzi błędu zwrócona przez API (jeśli dostępna).
    /// </summary>
    public string? ApiErrorBody { get; }

    /// <summary>
    /// Inicjalizuje nowy wyjątek z komunikatem.
    /// </summary>
    public FakturowniaException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Inicjalizuje nowy wyjątek z komunikatem i wyjątkiem wewnętrznym.
    /// </summary>
    public FakturowniaException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Inicjalizuje nowy wyjątek z komunikatem, kodem statusu HTTP i treścią błędu z API.
    /// </summary>
    public FakturowniaException(string message, int statusCode, string? apiErrorBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ApiErrorBody = apiErrorBody;
    }

    /// <summary>
    /// Inicjalizuje nowy wyjątek przy błędzie konfiguracji credentials.
    /// </summary>
    internal static FakturowniaException MissingCredentials() =>
        new("Brak credentials dla Fakturownia API. Przekaż FakturowniaAuthInfo lub skonfiguruj " +
            "FakturowniaOptions.ApiToken i FakturowniaOptions.Domain w appsettings.json.");
}
