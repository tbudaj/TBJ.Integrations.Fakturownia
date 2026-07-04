using System.Text.Json.Serialization;

namespace TBJ.Integrations.Fakturownia.Models.Clients;

/// <summary>
/// Żądanie aktualizacji danych klienta w Fakturownia API.
/// Przekaż tylko pola, które mają zostać zaktualizowane.
/// </summary>
public sealed class UpdateClientRequest
{
    /// <summary>Nazwa firmy / klienta.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>NIP.</summary>
    [JsonPropertyName("tax_no")]
    public string? TaxNo { get; set; }

    /// <summary>Ulica.</summary>
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    /// <summary>Kod pocztowy.</summary>
    [JsonPropertyName("post_code")]
    public string? PostCode { get; set; }

    /// <summary>Miasto.</summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>Kraj.</summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>E-mail.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>Telefon.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    /// <summary>Osoba kontaktowa.</summary>
    [JsonPropertyName("person")]
    public string? Person { get; set; }

    /// <summary>Bank.</summary>
    [JsonPropertyName("bank")]
    public string? Bank { get; set; }

    /// <summary>Numer konta bankowego.</summary>
    [JsonPropertyName("bank_account")]
    public string? BankAccount { get; set; }

    /// <summary>Uwagi / notatki.</summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }
}
