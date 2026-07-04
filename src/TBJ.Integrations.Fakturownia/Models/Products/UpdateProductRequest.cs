using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Models.Products;

/// <summary>
/// Żądanie aktualizacji produktu / usługi w Fakturownia API.
/// Przekaż tylko pola, które mają zostać zaktualizowane.
/// </summary>
public sealed class UpdateProductRequest
{
    /// <summary>Nazwa produktu / usługi.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Kod produktu (SKU).</summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>Opis produktu.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Cena jednostkowa netto.</summary>
    [JsonPropertyName("price_net")]
    public decimal? PriceNet { get; set; }

    /// <summary>Cena jednostkowa brutto.</summary>
    [JsonPropertyName("price_gross")]
    public decimal? PriceGross { get; set; }

    /// <summary>Stawka VAT (np. "23", "8", "0", "zw", "np").</summary>
    [JsonPropertyName("tax")]
    public string? Tax { get; set; }

    /// <summary>Jednostka miary.</summary>
    [JsonPropertyName("quantity_unit")]
    public string? QuantityUnit { get; set; }

    /// <summary>Numer PKWiU.</summary>
    [JsonPropertyName("pkwiu")]
    public string? Pkwiu { get; set; }

    /// <summary>Kod GTU.</summary>
    [JsonPropertyName("gtu_code")]
    public string? GtuCode { get; set; }
}
