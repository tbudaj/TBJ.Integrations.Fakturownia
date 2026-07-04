using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Models.Invoices;

/// <summary>
/// Pozycja (linia) na fakturze Fakturownia.
/// </summary>
public sealed class InvoicePosition
{
    /// <summary>Identyfikator pozycji (przy aktualizacji).</summary>
    public long? Id { get; set; }

    /// <summary>Identyfikator produktu z katalogu (opcjonalnie zamiast pełnych danych).</summary>
    public long? ProductId { get; set; }

    /// <summary>Nazwa produktu / usługi.</summary>
    public string? Name { get; set; }

    /// <summary>Dodatkowy opis pozycji.</summary>
    public string? Description { get; set; }

    /// <summary>Ilość.</summary>
    public decimal? Quantity { get; set; }

    /// <summary>Jednostka miary (np. szt., godz.).</summary>
    public string? QuantityUnit { get; set; }

    /// <summary>Cena jednostkowa netto.</summary>
    public decimal? PriceNet { get; set; }

    /// <summary>Cena jednostkowa brutto.</summary>
    public decimal? PriceGross { get; set; }

    /// <summary>Stawka VAT (np. 23, 8, 0, "zw", "np").</summary>
    public string? Tax { get; set; }

    /// <summary>Stawka VAT2 (dla szczególnych przypadków).</summary>
    public string? Tax2 { get; set; }

    /// <summary>Łączna cena netto pozycji.</summary>
    public decimal? TotalPriceNet { get; set; }

    /// <summary>Łączna cena brutto pozycji.</summary>
    public decimal? TotalPriceGross { get; set; }

    /// <summary>Kwota VAT dla pozycji.</summary>
    public decimal? TotalTax { get; set; }

    /// <summary>Rabat procentowy (gdy discount_kind = percent_unit).</summary>
    public decimal? DiscountPercent { get; set; }

    /// <summary>Rabat kwotowy.</summary>
    public decimal? Discount { get; set; }

    /// <summary>Numer PKWiU.</summary>
    public string? Pkwiu { get; set; }

    /// <summary>Kod GTU (dla JPK).</summary>
    public string? GtuCode { get; set; }
}
