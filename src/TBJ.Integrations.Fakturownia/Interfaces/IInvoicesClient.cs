using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Models.Invoices;

namespace TBJ.Integrations.Fakturownia.Interfaces;

/// <summary>
/// Klient do zarządzania fakturami w Fakturownia API.
/// </summary>
/// <remarks>
/// Wszystkie metody akceptują opcjonalny parametr <see cref="FakturowniaAuthInfo"/>.
/// Gdy <c>auth</c> jest <c>null</c>, używane są credentials z konfiguracji appsettings
/// (Scenariusz B — nasze własne konto Fakturownia).
/// </remarks>
public interface IInvoicesClient
{
    /// <summary>
    /// Pobiera listę faktur z zastosowaniem filtrów.
    /// </summary>
    /// <param name="filter">Filtry listy (klient, okres, stronicowanie itp.).</param>
    /// <param name="includePositions">Czy dołączyć pozycje faktur (<c>include_positions=true</c>) —
    /// bez tego flagi pozycje zwraca tylko endpoint pojedynczej faktury.</param>
    /// <param name="auth">Credentials tenanta lub null dla własnego konta.</param>
    /// <param name="ct">Token anulowania.</param>
    Task<IReadOnlyList<Invoice>> GetInvoicesAsync(
        InvoiceListFilter? filter = null,
        bool includePositions = false,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera pojedynczą fakturę po identyfikatorze.
    /// </summary>
    Task<Invoice> GetInvoiceAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera fakturę w formacie PDF jako tablicę bajtów.
    /// </summary>
    Task<byte[]> GetInvoicePdfAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Tworzy nową fakturę.
    /// </summary>
    Task<Invoice> CreateInvoiceAsync(
        CreateInvoiceRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Aktualizuje istniejącą fakturę.
    /// </summary>
    Task<Invoice> UpdateInvoiceAsync(
        long id,
        UpdateInvoiceRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Usuwa fakturę.
    /// </summary>
    Task DeleteInvoiceAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Wysyła fakturę do klienta e-mailem.
    /// </summary>
    Task SendByEmailAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Zmienia status faktury.
    /// </summary>
    /// <param name="id">Identyfikator faktury.</param>
    /// <param name="status">Nowy status: issued, sent, paid, partial, rejected.</param>
    /// <param name="auth">Credentials tenanta lub null dla własnego konta.</param>
    /// <param name="ct">Token anulowania.</param>
    Task<Invoice> ChangeStatusAsync(
        long id,
        string status,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);
}
