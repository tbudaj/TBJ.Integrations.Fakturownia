using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Products;

namespace TBJ.Integrations.Fakturownia.Clients;

/// <summary>
/// Implementacja klienta produktów / usług Fakturownia API.
/// </summary>
internal sealed class ProductsClient : IProductsClient
{
    private readonly FakturowniaHttpClient _http;

    public ProductsClient(FakturowniaHttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Product>> GetProductsAsync(
        string? name = null,
        string? code = null,
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
        if (code is not null) queryParams["code"] = code;

        return _http.GetAsync<IReadOnlyList<Product>>("products.json", auth, queryParams, ct);
    }

    /// <inheritdoc/>
    public Task<Product> GetProductAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<Product>($"products/{id}.json", auth, ct: ct);

    /// <inheritdoc/>
    public Task<Product> CreateProductAsync(
        CreateProductRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PostAsync<Product>("products.json", auth, new { product = request }, ct);

    /// <inheritdoc/>
    public Task<Product> UpdateProductAsync(
        long id,
        UpdateProductRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.PutAsync<Product>($"products/{id}.json", auth, new { product = request }, ct);

    /// <inheritdoc/>
    public Task DeleteProductAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.DeleteAsync($"products/{id}.json", auth, ct);
}
