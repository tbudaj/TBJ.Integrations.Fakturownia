using TBJ.Integrations.Fakturownia.Interfaces;

namespace TBJ.Integrations.Fakturownia;

/// <summary>
/// Implementacja głównego klienta integracji z Fakturownia API.
/// </summary>
internal sealed class FakturowniaClient : IFakturowniaClient
{
    /// <inheritdoc/>
    public IInvoicesClient Invoices { get; }

    /// <inheritdoc/>
    public IClientsClient Clients { get; }

    /// <inheritdoc/>
    public IProductsClient Products { get; }

    /// <inheritdoc/>
    public IPaymentsClient Payments { get; }

    /// <inheritdoc/>
    public ICategoriesClient Categories { get; }

    public FakturowniaClient(
        IInvoicesClient invoices,
        IClientsClient clients,
        IProductsClient products,
        IPaymentsClient payments,
        ICategoriesClient categories)
    {
        Invoices = invoices;
        Clients = clients;
        Products = products;
        Payments = payments;
        Categories = categories;
    }
}
