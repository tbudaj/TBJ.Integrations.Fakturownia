namespace TBJ.Integrations.Fakturownia.Models.Invoices;

/// <summary>
/// Filtry do pobierania listy faktur z Fakturownia API.
/// </summary>
public sealed class InvoiceListFilter
{
    /// <summary>
    /// Okres: this_month, last_month, this_year, last_year, all, lub "YYYY-MM" (konkretny miesiąc).
    /// Domyślnie: this_month.
    /// </summary>
    public string? Period { get; set; } = "this_month";

    /// <summary>Numer strony (1-bazowane). Domyślnie: 1.</summary>
    public int? Page { get; set; } = 1;

    /// <summary>Liczba rekordów na stronę (max 100). Domyślnie: 25.</summary>
    public int? PerPage { get; set; } = 25;

    /// <summary>Filtr po ID klienta.</summary>
    public long? ClientId { get; set; }

    /// <summary>Filtr po numerze zamówienia.</summary>
    public string? Oid { get; set; }

    /// <summary>Filtr po ID faktury nadrzędnej.</summary>
    public long? InvoiceId { get; set; }

    /// <summary>Filtr po ID faktury, na podstawie której wygenerowano bieżącą.</summary>
    public long? FromInvoiceId { get; set; }

    /// <summary>
    /// Rodzaj: income (przychody, domyślnie), expense (koszty/wydatki).
    /// Przekaż "no" dla wydatków: <c>income=no</c>.
    /// </summary>
    public string? Income { get; set; }

    /// <summary>Filtr po rodzaju dokumentu (np. vat, proforma, receipt).</summary>
    public string? Kind { get; set; }

    /// <summary>Filtr po statusie (issued, sent, paid, partial, rejected).</summary>
    public string? Status { get; set; }

    /// <summary>Filtr po nazwie nabywcy (wyszukiwanie częściowe).</summary>
    public string? BuyerName { get; set; }

    /// <summary>Konwertuje filtry do słownika query params.</summary>
    internal Dictionary<string, string?> ToQueryParams()
    {
        var dict = new Dictionary<string, string?>();

        if (Period is not null) dict["period"] = Period;
        if (Page is not null) dict["page"] = Page.ToString();
        if (PerPage is not null) dict["per_page"] = PerPage.ToString();
        if (ClientId is not null) dict["client_id"] = ClientId.ToString();
        if (Oid is not null) dict["oid"] = Oid;
        if (InvoiceId is not null) dict["invoice_id"] = InvoiceId.ToString();
        if (FromInvoiceId is not null) dict["from_invoice_id"] = FromInvoiceId.ToString();
        if (Income is not null) dict["income"] = Income;
        if (Kind is not null) dict["kind"] = Kind;
        if (Status is not null) dict["status"] = Status;
        if (BuyerName is not null) dict["buyer_name"] = BuyerName;

        return dict;
    }
}
