using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Models.Invoices;

/// <summary>
/// Żądanie aktualizacji istniejącej faktury.
/// Przekaż tylko pola, które mają zostać zaktualizowane.
/// </summary>
public sealed class UpdateInvoiceRequest
{
    /// <summary>Numer faktury.</summary>
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    /// <summary>Status faktury: issued, sent, paid, partial, rejected.</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>Data sprzedaży (YYYY-MM-DD).</summary>
    [JsonPropertyName("sell_date")]
    public string? SellDate { get; set; }

    /// <summary>Data wystawienia (YYYY-MM-DD).</summary>
    [JsonPropertyName("issue_date")]
    public string? IssueDate { get; set; }

    /// <summary>Termin płatności (YYYY-MM-DD).</summary>
    [JsonPropertyName("payment_to")]
    public string? PaymentTo { get; set; }

    /// <summary>Identyfikator nabywcy.</summary>
    [JsonPropertyName("client_id")]
    public long? ClientId { get; set; }

    /// <summary>Nazwa nabywcy.</summary>
    [JsonPropertyName("buyer_name")]
    public string? BuyerName { get; set; }

    /// <summary>NIP nabywcy.</summary>
    [JsonPropertyName("buyer_tax_no")]
    public string? BuyerTaxNo { get; set; }

    /// <summary>E-mail nabywcy.</summary>
    [JsonPropertyName("buyer_email")]
    public string? BuyerEmail { get; set; }

    /// <summary>Forma płatności.</summary>
    [JsonPropertyName("payment_type")]
    public string? PaymentType { get; set; }

    /// <summary>Waluta.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>Uwagi / notatki.</summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>Numer zamówienia.</summary>
    [JsonPropertyName("oid")]
    public string? Oid { get; set; }

    /// <summary>Pozycje faktury (aktualizacja / dodanie pozycji).</summary>
    [JsonPropertyName("positions")]
    public List<InvoicePosition>? Positions { get; set; }
}
