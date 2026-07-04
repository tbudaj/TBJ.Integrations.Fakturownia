using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Models.Products;

namespace TBJ.Integrations.Fakturownia.Interfaces;

/// <summary>
/// Klient do zarządzania produktami / usługami w Fakturownia API.
/// </summary>
/// <remarks>
/// Wszystkie metody akceptują opcjonalny parametr <see cref="FakturowniaAuthInfo"/>.
/// Gdy <c>auth</c> jest <c>null</c>, używane są credentials z konfiguracji appsettings.
/// </remarks>
public interface IProductsClient
{
    /// <summary>
    /// Pobiera listę produktów. Opcjonalnie filtruje po nazwie lub kodzie.
    /// </summary>
    Task<IReadOnlyList<Product>> GetProductsAsync(
        string? name = null,
        string? code = null,
        int page = 1,
        int perPage = 25,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera pojedynczy produkt po identyfikatorze.
    /// </summary>
    Task<Product> GetProductAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Tworzy nowy produkt / usługę.
    /// </summary>
    Task<Product> CreateProductAsync(
        CreateProductRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Aktualizuje istniejący produkt / usługę.
    /// </summary>
    Task<Product> UpdateProductAsync(
        long id,
        UpdateProductRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Usuwa produkt / usługę.
    /// </summary>
    Task DeleteProductAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);
}
