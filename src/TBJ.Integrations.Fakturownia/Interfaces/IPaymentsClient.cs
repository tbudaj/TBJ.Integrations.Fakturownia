using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Models.Payments;

namespace TBJ.Integrations.Fakturownia.Interfaces;

/// <summary>
/// Klient do zarządzania płatnościami w Fakturownia API.
/// </summary>
/// <remarks>
/// Wszystkie metody akceptują opcjonalny parametr <see cref="FakturowniaAuthInfo"/>.
/// Gdy <c>auth</c> jest <c>null</c>, używane są credentials z konfiguracji appsettings.
/// </remarks>
public interface IPaymentsClient
{
    /// <summary>
    /// Pobiera listę płatności. Opcjonalnie filtruje po fakturze.
    /// </summary>
    Task<IReadOnlyList<Payment>> GetPaymentsAsync(
        long? invoiceId = null,
        int page = 1,
        int perPage = 25,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera pojedynczą płatność po identyfikatorze.
    /// </summary>
    Task<Payment> GetPaymentAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);
}
