using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Invoices;

namespace TBJ.Integrations.Fakturownia.Clients;

/// <summary>
/// Implementacja klienta faktur Fakturownia API.
/// </summary>
internal sealed class InvoicesClient : IInvoicesClient
{
    private readonly FakturowniaHttpClient _http;

    public InvoicesClient(FakturowniaHttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Invoice>> GetInvoicesAsync(
        InvoiceListFilter? filter = null,
        bool includePositions = false,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default)
    {
        var queryParams = (filter ?? new InvoiceListFilter()).ToQueryParams();
        if (includePositions)
            queryParams["include_positions"] = "true";

        return _http.GetAsync<IReadOnlyList<Invoice>>("invoices.json", auth, queryParams, ct);
    }

    /// <inheritdoc/>
    public Task<Invoice> GetInvoiceAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<Invoice>($"invoices/{id}.json", auth, ct: ct);

    /// <inheritdoc/>
    public Task<byte[]> GetInvoicePdfAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetBytesAsync($"invoices/{id}.pdf", auth, ct: ct);

    /// <inheritdoc/>
    public Task<Invoice> CreateInvoiceAsync(
        CreateInvoiceRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PostAsync<Invoice>("invoices.json", auth, new { invoice = request }, ct);

    /// <inheritdoc/>
    public Task<Invoice> UpdateInvoiceAsync(
        long id,
        UpdateInvoiceRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PutAsync<Invoice>($"invoices/{id}.json", auth, new { invoice = request }, ct);

    /// <inheritdoc/>
    public Task DeleteInvoiceAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.DeleteAsync($"invoices/{id}.json", auth, ct);

    /// <inheritdoc/>
    public Task SendByEmailAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PostAsync($"invoices/{id}/send_by_email.json", auth, ct);

    /// <inheritdoc/>
    public Task<Invoice> ChangeStatusAsync(
        long id,
        string status,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PutAsync<Invoice>($"invoices/{id}.json", auth, new { invoice = new { status } }, ct);
}
