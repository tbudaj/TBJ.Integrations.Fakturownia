namespace TBJ.Integrations.Fakturownia.Models.Products;

/// <summary>
/// Produkt / usługa z katalogu Fakturownia API.
/// </summary>
public sealed class Product
{
    /// <summary>Identyfikator produktu.</summary>
    public long Id { get; set; }

    /// <summary>Nazwa produktu / usługi.</summary>
    public string? Name { get; set; }

    /// <summary>Kod produktu (SKU).</summary>
    public string? Code { get; set; }

    /// <summary>Opis produktu.</summary>
    public string? Description { get; set; }

    /// <summary>Cena jednostkowa netto.</summary>
    public decimal? PriceNet { get; set; }

    /// <summary>Cena jednostkowa brutto.</summary>
    public decimal? PriceGross { get; set; }

    /// <summary>Stawka VAT (np. "23", "8", "0", "zw").</summary>
    public string? Tax { get; set; }

    /// <summary>Jednostka miary (np. szt., godz.).</summary>
    public string? QuantityUnit { get; set; }

    /// <summary>Numer PKWiU.</summary>
    public string? Pkwiu { get; set; }

    /// <summary>Kod GTU.</summary>
    public string? GtuCode { get; set; }

    /// <summary>Bieżący stan magazynowy.</summary>
    public decimal? Stock { get; set; }

    /// <summary>Data i godzina utworzenia.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Data i godzina ostatniej modyfikacji.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
