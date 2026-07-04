using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Models.Invoices;

/// <summary>
/// Żądanie utworzenia nowej faktury w Fakturownia API.
/// </summary>
/// <remarks>
/// Wymagane pola minimalne: <see cref="Positions"/> z co najmniej jedną pozycją.
/// Pozostałe pola są opcjonalne — Fakturownia uzupełni je wartościami domyślnymi
/// (data bieżąca, 5-dniowy termin płatności, dane domyślnego działu itp.).
/// </remarks>
public sealed class CreateInvoiceRequest
{
    /// <summary>
    /// Rodzaj dokumentu (domyślnie: vat).
    /// Dostępne wartości: vat, proforma, receipt, advance, correction,
    /// vat_mp, invoice_other, vat_margin, kp, kw, expense, estimate.
    /// </summary>
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "vat";

    /// <summary>Numer faktury (null = automatyczna numeracja).</summary>
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    /// <summary>Data sprzedaży (YYYY-MM-DD). Domyślnie: dzień wystawienia.</summary>
    [JsonPropertyName("sell_date")]
    public string? SellDate { get; set; }

    /// <summary>Data wystawienia (YYYY-MM-DD). Domyślnie: dzień bieżący.</summary>
    [JsonPropertyName("issue_date")]
    public string? IssueDate { get; set; }

    /// <summary>Termin płatności (YYYY-MM-DD).</summary>
    [JsonPropertyName("payment_to")]
    public string? PaymentTo { get; set; }

    /// <summary>Liczba dni na płatność (alternatywa dla payment_to).</summary>
    [JsonPropertyName("payment_to_kind")]
    public int? PaymentToKind { get; set; }

    // === Sprzedawca ===

    /// <summary>Identyfikator działu (sprzedawcy). Brak = dział domyślny.</summary>
    [JsonPropertyName("department_id")]
    public long? DepartmentId { get; set; }

    /// <summary>Nazwa sprzedawcy (nadpisuje dział jeśli brak department_id).</summary>
    [JsonPropertyName("seller_name")]
    public string? SellerName { get; set; }

    /// <summary>NIP sprzedawcy.</summary>
    [JsonPropertyName("seller_tax_no")]
    public string? SellerTaxNo { get; set; }

    // === Nabywca ===

    /// <summary>Identyfikator klienta z katalogu (opcjonalnie zamiast pełnych danych).</summary>
    [JsonPropertyName("client_id")]
    public long? ClientId { get; set; }

    /// <summary>Nazwa nabywcy.</summary>
    [JsonPropertyName("buyer_name")]
    public string? BuyerName { get; set; }

    /// <summary>NIP nabywcy.</summary>
    [JsonPropertyName("buyer_tax_no")]
    public string? BuyerTaxNo { get; set; }

    /// <summary>Ulica nabywcy.</summary>
    [JsonPropertyName("buyer_street")]
    public string? BuyerStreet { get; set; }

    /// <summary>Kod pocztowy nabywcy.</summary>
    [JsonPropertyName("buyer_post_code")]
    public string? BuyerPostCode { get; set; }

    /// <summary>Miasto nabywcy.</summary>
    [JsonPropertyName("buyer_city")]
    public string? BuyerCity { get; set; }

    /// <summary>Kraj nabywcy.</summary>
    [JsonPropertyName("buyer_country")]
    public string? BuyerCountry { get; set; }

    /// <summary>E-mail nabywcy.</summary>
    [JsonPropertyName("buyer_email")]
    public string? BuyerEmail { get; set; }

    // === Płatność ===

    /// <summary>Forma płatności (transfer, card, cash, cheque, off).</summary>
    [JsonPropertyName("payment_type")]
    public string? PaymentType { get; set; }

    /// <summary>Waluta (domyślnie: PLN).</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    // === Rabat ===

    /// <summary>Czy faktura zawiera rabat.</summary>
    [JsonPropertyName("show_discount")]
    public bool? ShowDiscount { get; set; }

    /// <summary>Rodzaj rabatu: percent_unit (procentowy), amount (kwotowy).</summary>
    [JsonPropertyName("discount_kind")]
    public string? DiscountKind { get; set; }

    // === Inne ===

    /// <summary>Numer zamówienia.</summary>
    [JsonPropertyName("oid")]
    public string? Oid { get; set; }

    /// <summary>Uwagi / notatki na fakturze.</summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>Identyfikator faktury nadrzędnej (dla faktur zaliczkowych/końcowych).</summary>
    [JsonPropertyName("from_invoice_id")]
    public long? FromInvoiceId { get; set; }

    /// <summary>Pozycje faktury. Wymagane co najmniej jedna.</summary>
    [JsonPropertyName("positions")]
    public required List<InvoicePosition> Positions { get; set; }
}
