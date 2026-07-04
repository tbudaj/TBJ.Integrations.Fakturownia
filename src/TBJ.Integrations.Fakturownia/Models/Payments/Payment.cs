namespace TBJ.Integrations.Fakturownia.Models.Payments;

/// <summary>
/// Płatność powiązana z fakturą w Fakturownia API.
/// </summary>
public sealed class Payment
{
    /// <summary>Identyfikator płatności.</summary>
    public long Id { get; set; }

    /// <summary>Identyfikator faktury.</summary>
    public long? InvoiceId { get; set; }

    /// <summary>Kwota płatności.</summary>
    public decimal? Price { get; set; }

    /// <summary>Waluta.</summary>
    public string? Currency { get; set; }

    /// <summary>Data płatności (YYYY-MM-DD).</summary>
    public string? Date { get; set; }

    /// <summary>Forma płatności.</summary>
    public string? PaymentType { get; set; }

    /// <summary>Uwagi.</summary>
    public string? Note { get; set; }

    /// <summary>Data i godzina utworzenia.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Data i godzina ostatniej modyfikacji.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
