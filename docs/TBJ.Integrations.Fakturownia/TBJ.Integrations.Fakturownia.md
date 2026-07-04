# TBJ.Integrations.Fakturownia

Typowany klient .NET dla Fakturownia.pl API — faktury, kontrahenci, produkty, płatności, kategorie.

## Instalacja

```bash
dotnet add package TBJ.Integrations.Fakturownia
```

## Rejestracja w DI

```csharp
builder.Services.AddFakturownia(builder.Configuration);
```

```json
{
  "Fakturownia": {
    "Timeout": "00:00:30",
    "ApiToken": "twoj-token",
    "Domain": "twoja-firma"
  }
}
```

## Wielotenantowość

Biblioteka obsługuje dwa scenariusze:

- **Scenariusz A — konto tenanta:** `FakturowniaAuthInfo` przekazywane per-żądanie.
- **Scenariusz B — nasze konto:** `ApiToken` i `Domain` z `FakturowniaOptions` jako fallback.

## Klienci

| Klient | Odpowiedzialność |
|---|---|
| `IInvoicesClient` | Faktury |
| `IClientsClient` | Kontrahenci |
| `IProductsClient` | Produkty |
| `IPaymentsClient` | Płatności |
| `ICategoriesClient` | Kategorie |
| `IFakturowniaClient` | Fasada agregująca wszystkie klientów |

## Przykład użycia

```csharp
public class InvoiceService(IFakturowniaClient fakturownia)
{
    public async Task CreateInvoiceAsync()
    {
        var invoice = await fakturownia.Invoices.CreateInvoiceAsync(new CreateInvoiceRequest
        {
            Kind = "vat",
            Number = "FV/1/2024",
            SellDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            // ...
        });

        Console.WriteLine($"Utworzono fakturę {invoice.Id}");
    }
}
```

## Paginacja

Fakturownia API używa numerycznej paginacji (parametry `page` i `per_page`), którą ustawisz przez `InvoiceListFilter.Page` oraz `InvoiceListFilter.PerPage`.

## Obsługa błędów

Błędy API są konwertowane do `FakturowniaException`:

```csharp
try
{
    var invoice = await fakturownia.Invoices.GetInvoiceAsync(999);
}
catch (FakturowniaException ex)
{
    Console.WriteLine($"Błąd Fakturownia: {ex.Message}");
}
```

## Cykl życia serwisów

| Serwis | Lifetime |
|---|---|
| `IFakturowniaClient` | `Scoped` |
| Domenowi klienci | `Scoped` |
| `HttpClient` | przez `IHttpClientFactory` |
