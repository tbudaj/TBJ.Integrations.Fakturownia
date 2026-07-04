using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Payments;

namespace TBJ.Integrations.Fakturownia.Clients;

/// <summary>
/// Implementacja klienta płatności Fakturownia API.
/// </summary>
internal sealed class PaymentsClient : IPaymentsClient
{
    private readonly FakturowniaHttpClient _http;

    public PaymentsClient(FakturowniaHttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Payment>> GetPaymentsAsync(
        long? invoiceId = null,
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

        if (invoiceId is not null) queryParams["invoice_id"] = invoiceId.ToString();

        return _http.GetAsync<IReadOnlyList<Payment>>("payments.json", auth, queryParams, ct);
    }

    /// <inheritdoc/>
    public Task<Payment> GetPaymentAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<Payment>($"payments/{id}.json", auth, ct: ct);
}
