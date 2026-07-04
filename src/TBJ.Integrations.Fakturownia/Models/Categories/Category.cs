namespace TBJ.Integrations.Fakturownia.Models.Categories;

/// <summary>
/// Kategoria w Fakturownia API.
/// </summary>
public sealed class Category
{
    /// <summary>Identyfikator kategorii.</summary>
    public long Id { get; set; }

    /// <summary>Nazwa kategorii.</summary>
    public string? Name { get; set; }

    /// <summary>Opis kategorii.</summary>
    public string? Description { get; set; }

    /// <summary>Data i godzina utworzenia.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Data i godzina ostatniej modyfikacji.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
