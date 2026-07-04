using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Categories;

namespace TBJ.Integrations.Fakturownia.Clients;

/// <summary>
/// Implementacja klienta kategorii Fakturownia API.
/// </summary>
internal sealed class CategoriesClient : ICategoriesClient
{
    private readonly FakturowniaHttpClient _http;

    public CategoriesClient(FakturowniaHttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Category>> GetCategoriesAsync(
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<IReadOnlyList<Category>>("categories.json", auth, ct: ct);

    /// <inheritdoc/>
    public Task<Category> GetCategoryAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default) =>
        _http.GetAsync<Category>($"categories/{id}.json", auth, ct: ct);
}
