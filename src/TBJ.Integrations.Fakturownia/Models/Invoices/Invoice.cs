using System.Text.Json.Serialization;
using TBJ.Integrations.Fakturownia.Internal;

namespace TBJ.Integrations.Fakturownia.Models.Invoices;

/// <summary>
/// Faktura z Fakturownia API.
/// </summary>
public sealed class Invoice
{
    /// <summary>Identyfikator faktury.</summary>
    public long Id { get; set; }

    /// <summary>Numer faktury (np. "FV 1/2026").</summary>
    public string? Number { get; set; }

    /// <summary>
    /// Rodzaj dokumentu: vat, proforma, receipt, advance, correction,
    /// vat_mp, invoice_other, vat_margin, kp, kw, expense, estimate, etc.
    /// </summary>
    public string? Kind { get; set; }

    /// <summary>Status faktury: issued, sent, paid, partial, rejected.</summary>
    public string? Status { get; set; }

    // === Daty ===

    /// <summary>Data sprzedaży (YYYY-MM-DD).</summary>
    public string? SellDate { get; set; }

    /// <summary>Data wystawienia (YYYY-MM-DD).</summary>
    public string? IssueDate { get; set; }

    /// <summary>Termin płatności (YYYY-MM-DD).</summary>
    public string? PaymentTo { get; set; }

    // === Sprzedawca ===

    /// <summary>Identyfikator działu (sprzedawcy).</summary>
    public long? DepartmentId { get; set; }

    /// <summary>Nazwa sprzedawcy.</summary>
    public string? SellerName { get; set; }

    /// <summary>NIP sprzedawcy.</summary>
    public string? SellerTaxNo { get; set; }

    /// <summary>Ulica sprzedawcy.</summary>
    public string? SellerStreet { get; set; }

    /// <summary>Kod pocztowy sprzedawcy.</summary>
    public string? SellerPostCode { get; set; }

    /// <summary>Miasto sprzedawcy.</summary>
    public string? SellerCity { get; set; }

    /// <summary>Kraj sprzedawcy.</summary>
    public string? SellerCountry { get; set; }

    // === Nabywca ===

    /// <summary>Identyfikator klienta.</summary>
    public long? ClientId { get; set; }

    /// <summary>Nazwa nabywcy.</summary>
    public string? BuyerName { get; set; }

    /// <summary>NIP nabywcy.</summary>
    public string? BuyerTaxNo { get; set; }

    /// <summary>Ulica nabywcy.</summary>
    public string? BuyerStreet { get; set; }

    /// <summary>Kod pocztowy nabywcy.</summary>
    public string? BuyerPostCode { get; set; }

    /// <summary>Miasto nabywcy.</summary>
    public string? BuyerCity { get; set; }

    /// <summary>Kraj nabywcy.</summary>
    public string? BuyerCountry { get; set; }

    /// <summary>E-mail nabywcy.</summary>
    public string? BuyerEmail { get; set; }

    // === Wartości finansowe ===

    /// <summary>Suma netto (w groszach × 100).</summary>
    public decimal? PriceNet { get; set; }

    /// <summary>Suma brutto (w groszach × 100).</summary>
    public decimal? PriceGross { get; set; }

    /// <summary>Suma VAT.</summary>
    public decimal? PriceTax { get; set; }

    /// <summary>Kwota zapłacona.</summary>
    public decimal? PaidTotal { get; set; }

    /// <summary>Waluta (np. PLN, EUR).</summary>
    public string? Currency { get; set; }

    // === Płatność ===

    /// <summary>Forma płatności (transfer, card, cash, etc.).</summary>
    public string? PaymentType { get; set; }

    /// <summary>Kwota zapłacona (pole <c>paid</c>).</summary>
    [JsonConverter(typeof(FlexibleDecimalConverter))]
    public decimal? Paid { get; set; }

    /// <summary>Data zapłaty (YYYY-MM-DD), pusta gdy nieopłacona.</summary>
    public string? PaidDate { get; set; }

    /// <summary>Link do płatności online (tylko gdy na koncie włączone są płatności online; zwykle w szczegółach faktury).</summary>
    public string? PaymentUrl { get; set; }

    // === Rabat ===

    /// <summary>Czy faktura zawiera rabat.</summary>
    public bool? ShowDiscount { get; set; }

    /// <summary>Rodzaj rabatu: percent_unit, amount.</summary>
    public string? DiscountKind { get; set; }

    // === Inne ===

    /// <summary>Numer zamówienia powiązanego.</summary>
    public string? Oid { get; set; }

    /// <summary>Uwagi / notatki na fakturze.</summary>
    public string? Note { get; set; }

    /// <summary>Data i godzina utworzenia.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Data i godzina ostatniej modyfikacji.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Identyfikator faktury nadrzędnej (np. zaliczkowej).</summary>
    public long? FromInvoiceId { get; set; }

    /// <summary>Pozycje faktury.</summary>
    public List<InvoicePosition>? Positions { get; set; }

    /// <summary>Adres URL podglądu faktury.</summary>
    public string? ViewUrl { get; set; }

    /// <summary>Token publicznego podglądu: <c>https://{domena}.fakturownia.pl/invoice/{token}</c> oraz <c>.pdf</c>.</summary>
    public string? Token { get; set; }
}
