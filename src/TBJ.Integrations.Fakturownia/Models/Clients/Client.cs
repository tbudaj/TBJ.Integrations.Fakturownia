namespace TBJ.Integrations.Fakturownia.Models.Clients;

/// <summary>
/// Klient (kontrahent) w Fakturownia API.
/// </summary>
public sealed class Client
{
    /// <summary>Identyfikator klienta.</summary>
    public long Id { get; set; }

    /// <summary>Nazwa firmy / klienta.</summary>
    public string? Name { get; set; }

    /// <summary>NIP.</summary>
    public string? TaxNo { get; set; }

    /// <summary>Ulica.</summary>
    public string? Street { get; set; }

    /// <summary>Kod pocztowy.</summary>
    public string? PostCode { get; set; }

    /// <summary>Miasto.</summary>
    public string? City { get; set; }

    /// <summary>Kraj.</summary>
    public string? Country { get; set; }

    /// <summary>E-mail.</summary>
    public string? Email { get; set; }

    /// <summary>Telefon.</summary>
    public string? Phone { get; set; }

    /// <summary>Osoba kontaktowa.</summary>
    public string? Person { get; set; }

    /// <summary>Bank.</summary>
    public string? Bank { get; set; }

    /// <summary>Numer konta bankowego.</summary>
    public string? BankAccount { get; set; }

    /// <summary>Numer konta bankowego IBAN.</summary>
    public string? BankAccountId { get; set; }

    /// <summary>Uwagi / notatki.</summary>
    public string? Note { get; set; }

    /// <summary>Data i godzina utworzenia.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Data i godzina ostatniej modyfikacji.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
