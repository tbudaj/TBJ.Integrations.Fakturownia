using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Models.Categories;

namespace TBJ.Integrations.Fakturownia.Interfaces;

/// <summary>
/// Klient do zarządzania kategoriami w Fakturownia API.
/// </summary>
/// <remarks>
/// Wszystkie metody akceptują opcjonalny parametr <see cref="FakturowniaAuthInfo"/>.
/// Gdy <c>auth</c> jest <c>null</c>, używane są credentials z konfiguracji appsettings.
/// </remarks>
public interface ICategoriesClient
{
    /// <summary>
    /// Pobiera listę kategorii.
    /// </summary>
    Task<IReadOnlyList<Category>> GetCategoriesAsync(
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera pojedynczą kategorię po identyfikatorze.
    /// </summary>
    Task<Category> GetCategoryAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);
}
