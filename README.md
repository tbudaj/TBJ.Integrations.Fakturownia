# TBJ.Integrations.Fakturownia

[![build](https://github.com/tbudaj/TBJ.Integrations.Fakturownia/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/tbudaj/TBJ.Integrations.Fakturownia/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/TBJ.Integrations.Fakturownia)](https://www.nuget.org/packages/TBJ.Integrations.Fakturownia)

Biblioteka klienta .NET 8, 9, 10 dla **Fakturownia.pl API** — wystawianie i zarządzanie fakturami online.

Projekt jest wzorowany na `TBJ.Integrations.Shipping.*` i `TBJ.Integrations.MF.KSeF`. Dostarcza typowanych abstrakcji do komunikacji z Fakturownia API: faktury, kontrahenci, produkty, płatności, kategorie — z pełnym wsparciem wielotenantowości.

---

## Spis treści

- [Wymagania](#wymagania)
- [Model wielotenantowości](#model-wielotenantowości)
- [Jak uzyskać dane dostępowe](#jak-uzyskać-dane-dostępowe)
- [Rejestracja w DI](#rejestracja-w-di)
- [Konfiguracja appsettings.json](#konfiguracja-appsettingsjson)
- [Zakres funkcjonalności](#zakres-funkcjonalności)
- [Przykłady użycia](#przykłady-użycia)
  - [Faktury](#faktury)
  - [Kontrahenci](#kontrahenci)
  - [Produkty](#produkty)
  - [Płatności](#płatności)
  - [Kategorie](#kategorie)
- [Paginacja](#paginacja)
- [Obsługa błędów](#obsługa-błędów)
- [Architektura](#architektura)
- [Znane ograniczenia i kolejne kroki](#znane-ograniczenia-i-kolejne-kroki)

---

## Wymagania

- .NET 8, 9, 10
- Konto w [Fakturownia.pl](https://fakturownia.pl)
- API token z ustawień konta: **Ustawienia → Ustawienia konta → Integracja → Kod autoryzacyjny API**

---

## Model wielotenantowości

Biblioteka obsługuje **dwa scenariusze uwierzytelniania** obsługiwane przez ten sam kod — wybór następuje automatycznie na podstawie obecności `FakturowniaAuthInfo`:

### Scenariusz A — konto tenanta

Tenant posiada własne konto Fakturownia. Credentials (`ApiToken`, `Domain`) przechowywane są w bazie danych aplikacji nadrzędnej i przekazywane **per-żądanie** przez `FakturowniaAuthInfo`.

```
Tenant A → FakturowniaAuthInfo { ApiToken = "tok_A", Domain = "firma-a" }
Tenant B → FakturowniaAuthInfo { ApiToken = "tok_B", Domain = "firma-b" }
```

> **Nigdy nie umieszczaj tokenów tenantów w `appsettings.json`.** Przechowuj je w bazie danych i przekazuj przez `FakturowniaAuthInfo`.

### Scenariusz B — nasze konto (faktury własne)

Chcemy wyświetlić tenantowi faktury wystawione z **naszego** konta Fakturownia (np. fakturę za usługę TBJ). Credentials ładowane są z `appsettings.json` i używane automatycznie gdy wywołujący **nie przekazuje** `FakturowniaAuthInfo` (parametr `auth = null`).

```
Brak auth → FakturowniaOptions.ApiToken + FakturowniaOptions.Domain z appsettings.json
```

### Logika rozwiązywania credentials

```
Wywołanie metody z auth: FakturowniaAuthInfo?
         │
         ▼
   auth != null?
   ┌─────────────────────────────────────────┐
   │ TAK → credentials tenanta z bazy danych  │  ← Scenariusz A
   │       auth.ApiToken + auth.Domain        │
   └─────────────────────────────────────────┘
         │
         ▼
   auth == null?
   ┌─────────────────────────────────────────────────────┐
   │ TAK → credentials z FakturowniaOptions (appsettings) │  ← Scenariusz B
   │       options.ApiToken + options.Domain              │
   └─────────────────────────────────────────────────────┘
         │
         ▼
   Brak obu → FakturowniaException (MissingCredentials)
```

---

## Jak uzyskać dane dostępowe

### API token

1. Zaloguj się do konta Fakturownia
2. Przejdź do: **Ustawienia → Ustawienia konta → Integracja**
3. Skopiuj wartość pola **Kod autoryzacyjny API**

### Domain (prefiks subdomeny)

- Prefiks subdomeny konta, np. dla `https://moja-firma.fakturownia.pl` wartość to `moja-firma`
- Widoczny w pasku adresu przeglądarki po zalogowaniu do konta

### Środowisko testowe

Fakturownia nie udostępnia oddzielnego środowiska sandbox. Do testów zaleca się:
- Założenie osobnego konta testowego na fakturownia.pl
- Ustawianie go jako konfiguracji w `appsettings.Development.json`

---

## Rejestracja w DI

### Wariant 1 — konfiguracja z `appsettings.json` (zalecany)

```csharp
// Program.cs
builder.Services.AddFakturownia(builder.Configuration);
```

### Wariant 2 — konfiguracja inline

```csharp
// Scenariusz A — tylko infrastruktura, credentials przekazywane per-request:
builder.Services.AddFakturownia();

// Scenariusz B — nasze konto jako fallback:
builder.Services.AddFakturownia(opt =>
{
    opt.ApiToken = "xxx-nasz-api-token";
    opt.Domain   = "nasza-firma";
    opt.Timeout  = TimeSpan.FromSeconds(60);
});
```

---

## Konfiguracja appsettings.json

```jsonc
// appsettings.json
{
  "Fakturownia": {
    "Timeout":  "00:00:30",

    // Scenariusz B — nasze własne konto (opcjonalne).
    // Używane tylko gdy auth nie jest przekazane per-request.
    "ApiToken": "xxx-nasz-api-token",
    "Domain":   "nasza-firma"
  }
}
```

```jsonc
// appsettings.Development.json — konto testowe
{
  "Fakturownia": {
    "ApiToken": "xxx-testowy-token",
    "Domain":   "nasza-firma-test"
  }
}
```

### Opis parametrów

| Parametr | Wymagany | Domyślna wartość | Opis |
|----------|----------|------------------|------|
| `Timeout` | nie | `00:00:30` | Timeout żądań HTTP (`hh:mm:ss`) |
| `ApiToken` | nie* | — | API token naszego konta (Scenariusz B) |
| `Domain` | nie* | — | Prefiks subdomeny naszego konta (Scenariusz B) |

*Wymagane jeśli używasz Scenariusza B bez przekazywania `FakturowniaAuthInfo`.

---

## Zakres funkcjonalności

| Obszar | Operacje |
|--------|---------|
| **Faktury** | lista (z filtrami), pobierz po ID, PDF, utwórz, aktualizuj, usuń, wyślij e-mail, zmień status |
| **Kontrahenci** | lista (filtr po nazwie/NIP), pobierz po ID, utwórz, aktualizuj, usuń |
| **Produkty / usługi** | lista (filtr po nazwie/kodzie), pobierz po ID, utwórz, aktualizuj, usuń |
| **Płatności** | lista (filtr po fakturze), pobierz po ID |
| **Kategorie** | lista, pobierz po ID |

---

## Przykłady użycia

### Wstrzyknięcie fasady

```csharp
public class RozliczeniaService
{
    private readonly IFakturowniaClient _fakturownia;

    public RozliczeniaService(IFakturowniaClient fakturownia)
    {
        _fakturownia = fakturownia;
    }
}
```

### Faktury

#### Pobranie listy faktur

```csharp
// Scenariusz B — nasze faktury z bieżącego miesiąca (auth pomijamy)
var faktury = await _fakturownia.Invoices.GetInvoicesAsync();

// Scenariusz A — faktury tenanta z konkretnego miesiąca
var authTenanta = new FakturowniaAuthInfo
{
    ApiToken = tenantFromDb.FakturowniaToken,
    Domain   = tenantFromDb.FakturowniaPrefix,
};

var faktury = await _fakturownia.Invoices.GetInvoicesAsync(
    filter: new InvoiceListFilter
    {
        Period  = "2026-05",    // konkretny miesiąc, lub: this_month, last_month, this_year, all
        Page    = 1,
        PerPage = 50,
        Status  = "paid",       // opcjonalnie: issued, sent, paid, partial, rejected
        Order   = "issue_date.desc", // opcjonalne sortowanie: issue_date, payment_to, paid_date,
                                     // number, updated_at; sufiks .desc = malejąco
    },
    auth: authTenanta
);
```

#### Pobranie faktury i jej PDF

```csharp
// Pobranie szczegółów faktury
var faktura = await _fakturownia.Invoices.GetInvoiceAsync(123456, authTenanta);
Console.WriteLine(faktura.Number);     // np. "FV 5/06/2026"
Console.WriteLine(faktura.PriceGross); // kwota brutto
Console.WriteLine(faktura.Paid);       // kwota zapłacona
Console.WriteLine(faktura.PaidDate);   // data zapłaty (YYYY-MM-DD), pusta gdy nieopłacona
Console.WriteLine(faktura.PaymentUrl); // link do płatności online (gdy włączone na koncie)
Console.WriteLine(faktura.Token);      // token publicznego podglądu

// Pobranie PDF
byte[] pdf = await _fakturownia.Invoices.GetInvoicePdfAsync(123456, authTenanta);
await File.WriteAllBytesAsync("faktura.pdf", pdf);
```

Pola płatności w modelu `Invoice`:

- `Paid` — kwota zapłacona (API zwraca ją jako string, np. `"0,00"` lub `"123.45"` — konwerter obsługuje oba formaty oraz `null`)
- `PaidDate` — data zapłaty (YYYY-MM-DD), pusta gdy faktura nieopłacona
- `PaymentUrl` — link do płatności online, dostępny tylko gdy na koncie włączone są płatności online
- `Token` — token publicznego podglądu: `https://{domena}.fakturownia.pl/invoice/{token}`, dopisek `.pdf` daje PDF

#### Wystawienie nowej faktury

```csharp
var nowaFaktura = await _fakturownia.Invoices.CreateInvoiceAsync(
    new CreateInvoiceRequest
    {
        Kind      = "vat",                 // domyślnie; inne: proforma, receipt, advance, ...
        SellDate  = "2026-06-27",
        PaymentTo = "2026-07-04",          // lub PaymentToKind = 7 (7 dni)

        // Nabywca — wystarczy ClientId jeśli klient istnieje w katalogu:
        ClientId  = 111,
        // Lub pełne dane:
        BuyerName   = "Klient SA",
        BuyerTaxNo  = "1234567890",
        BuyerStreet = "ul. Testowa 1",
        BuyerCity   = "Warszawa",

        PaymentType = "transfer",          // transfer, card, cash, cheque

        Positions = new List<InvoicePosition>
        {
            new()
            {
                ProductId = 333,           // lub pełne dane pozycji:
                Name      = "Usługa XYZ",
                Tax       = "23",          // stawka VAT; lub "0", "zw", "np"
                PriceNet  = 100.00m,
                Quantity  = 2,
            },
            new()
            {
                Name      = "Hosting miesięczny",
                Tax       = "23",
                PriceNet  = 49.00m,
                Quantity  = 1,
                QuantityUnit = "miesiąc",
            },
        },
    },
    auth: authTenanta
);

Console.WriteLine(nowaFaktura.Id);
Console.WriteLine(nowaFaktura.Number);    // np. "FV 6/06/2026"
Console.WriteLine(nowaFaktura.ViewUrl);   // link do podglądu w Fakturownia
```

#### Wysyłka e-mailem i zmiana statusu

```csharp
// Wyślij do klienta e-mailem (na adres z faktury)
await _fakturownia.Invoices.SendByEmailAsync(nowaFaktura.Id, authTenanta);

// Zmień status na opłacona
await _fakturownia.Invoices.ChangeStatusAsync(nowaFaktura.Id, "paid", authTenanta);

// Aktualizacja danych
var zaktualizowana = await _fakturownia.Invoices.UpdateInvoiceAsync(
    nowaFaktura.Id,
    new UpdateInvoiceRequest { Note = "Uwaga wewnętrzna" },
    authTenanta
);
```

#### Usunięcie faktury

```csharp
await _fakturownia.Invoices.DeleteInvoiceAsync(nowaFaktura.Id, authTenanta);
```

#### Faktura dla istniejącego klienta i produktu

Gdy masz identyfikatory klienta i produktu z katalogu Fakturownia, nie musisz przekazywać pełnych danych:

```csharp
var faktura = await _fakturownia.Invoices.CreateInvoiceAsync(
    new CreateInvoiceRequest
    {
        ClientId     = 111,
        DepartmentId = 222,   // opcjonalnie — dział/sprzedawca
        PaymentToKind = 5,    // 5 dni
        Positions = new List<InvoicePosition>
        {
            new() { ProductId = 333, Quantity = 2 }
        },
    },
    auth: authTenanta
);
```

---

### Kontrahenci

```csharp
// Lista klientów — filtruj po NIP
var klienci = await _fakturownia.Clients.GetClientsAsync(
    taxNo: "1234567890",
    auth: authTenanta
);

// Pobierz klienta po ID
var klient = await _fakturownia.Clients.GetClientAsync(111, authTenanta);

// Utwórz klienta
var nowyKlient = await _fakturownia.Clients.CreateClientAsync(
    new CreateClientRequest
    {
        Name      = "Firma ABC Sp. z o.o.",
        TaxNo     = "1234567890",
        Street    = "ul. Testowa 1",
        PostCode  = "00-001",
        City      = "Warszawa",
        Email     = "faktury@firma.pl",
        Phone     = "+48123456789",
    },
    auth: authTenanta
);

// Aktualizacja
await _fakturownia.Clients.UpdateClientAsync(
    nowyKlient.Id,
    new UpdateClientRequest { Email = "nowy@firma.pl" },
    authTenanta
);

// Usunięcie
await _fakturownia.Clients.DeleteClientAsync(nowyKlient.Id, authTenanta);
```

---

### Produkty

```csharp
// Lista produktów — filtruj po kodzie SKU
var produkty = await _fakturownia.Products.GetProductsAsync(
    code: "SKU-001",
    auth: authTenanta
);

// Utwórz produkt / usługę
var nowyProdukt = await _fakturownia.Products.CreateProductAsync(
    new CreateProductRequest
    {
        Name         = "Usługa konsultingowa",
        Tax          = "23",
        PriceNet     = 200.00m,
        QuantityUnit = "godz.",
        Pkwiu        = "62.02.30.0",
    },
    auth: authTenanta
);

// Aktualizacja ceny
await _fakturownia.Products.UpdateProductAsync(
    nowyProdukt.Id,
    new UpdateProductRequest { PriceNet = 250.00m },
    authTenanta
);
```

---

### Płatności

```csharp
// Płatności dla konkretnej faktury
var platnosci = await _fakturownia.Payments.GetPaymentsAsync(
    invoiceId: 123456,
    auth: authTenanta
);

// Szczegóły płatności
var platnosc = await _fakturownia.Payments.GetPaymentAsync(789, authTenanta);
Console.WriteLine($"{platnosc.Date}: {platnosc.Price} {platnosc.Currency}");
```

---

### Kategorie

```csharp
var kategorie = await _fakturownia.Categories.GetCategoriesAsync(authTenanta);

foreach (var kat in kategorie)
    Console.WriteLine($"{kat.Id}: {kat.Name}");
```

---

## Paginacja

Fakturownia API zwraca listy z paginacją numeryczną (`page` + `per_page`). Maksymalny rozmiar strony to **100** rekordów.

```csharp
// Pobierz wszystkie faktury iterując po stronach
var wszystkieFaktury = new List<Invoice>();
int page = 1;

while (true)
{
    var strona = await _fakturownia.Invoices.GetInvoicesAsync(
        filter: new InvoiceListFilter
        {
            Period  = "this_year",
            Page    = page,
            PerPage = 100,
        },
        auth: authTenanta
    );

    if (strona.Count == 0)
        break;

    wszystkieFaktury.AddRange(strona);

    if (strona.Count < 100)
        break;

    page++;
}
```

---

## Obsługa błędów

Wszystkie metody rzucają `FakturowniaException` przy błędach API lub problemach z połączeniem:

```csharp
try
{
    var faktura = await _fakturownia.Invoices.GetInvoiceAsync(123456, authTenanta);
}
catch (FakturowniaException ex) when (ex.StatusCode == 404)
{
    _logger.LogWarning("Faktura nie istnieje: {Id}", 123456);
}
catch (FakturowniaException ex)
{
    _logger.LogError(ex,
        "Błąd Fakturownia API: HTTP {StatusCode}, body: {Body}",
        ex.StatusCode,
        ex.ApiErrorBody);
    throw;
}
```

### Typowe kody statusu HTTP

| StatusCode | Znaczenie | Typyczna przyczyna |
|------------|-----------|-------------------|
| `401` | Unauthorized | Nieprawidłowy `ApiToken` |
| `404` | Not Found | Dokument o podanym ID nie istnieje |
| `422` | Unprocessable Entity | Błąd walidacji (np. brak pozycji na fakturze) |
| `429` | Too Many Requests | Przekroczono limit żądań API |
| `null` | Błąd połączenia | Timeout lub brak dostępu do sieci |

---

## Architektura

```
TBJ.Integrations.Fakturownia/
├── Auth/
│   └── FakturowniaAuthInfo.cs         # credentials per-request (ApiToken + Domain)
├── Configuration/
│   └── FakturowniaOptions.cs          # opcje infrastrukturalne + fallback credentials
├── Exceptions/
│   └── FakturowniaException.cs        # wyjątek z StatusCode i ApiErrorBody
├── Internal/
│   └── FakturowniaHttpClient.cs       # HTTP wrapper — logika fallback auth,
│                                      # dynamiczny URL, api_token w query/body
├── Models/
│   ├── Invoices/                      # Invoice, InvoicePosition,
│   │                                  # CreateInvoiceRequest, UpdateInvoiceRequest,
│   │                                  # InvoiceListFilter
│   ├── Clients/                       # Client, CreateClientRequest, UpdateClientRequest
│   ├── Products/                      # Product, CreateProductRequest, UpdateProductRequest
│   ├── Payments/                      # Payment
│   └── Categories/                    # Category
├── Interfaces/
│   ├── IInvoicesClient.cs
│   ├── IClientsClient.cs
│   ├── IProductsClient.cs
│   ├── IPaymentsClient.cs
│   └── ICategoriesClient.cs
├── Clients/                           # wewnętrzne implementacje interfejsów
│   ├── InvoicesClient.cs
│   ├── ClientsClient.cs
│   ├── ProductsClient.cs
│   ├── PaymentsClient.cs
│   └── CategoriesClient.cs
├── IFakturowniaClient.cs              # fasada agregująca wszystkie klienty
├── FakturowniaClient.cs               # implementacja fasady (internal)
└── FakturowniaExtensions.cs           # DI: AddFakturownia()
```

### Szczegóły techniczne `FakturowniaHttpClient`

- **Brak globalnego `BaseAddress`** — URL budowany dynamicznie per-request jako `https://{domain}.fakturownia.pl`, ponieważ każdy tenant ma inną subdomenę
- **`api_token` w query string** dla żądań `GET` i `DELETE`
- **`api_token` w JSON body** dla żądań `POST` i `PUT` (wymóg API Fakturownia)
- **JSON** z `snake_case` (właściwości API używają `underscore_notation`)
- **`JsonIgnoreCondition.WhenWritingNull`** — pola `null` nie są wysyłane, co pozwala na częściowe aktualizacje przez `PUT`

---

## Znane ograniczenia i kolejne kroki

| Obszar | Stan | Uwagi |
|--------|------|-------|
| Faktury — zakres podstawowy | ✅ zaimplementowane | CRUD, PDF, email, status |
| Kontrahenci — zakres podstawowy | ✅ zaimplementowane | CRUD |
| Produkty — zakres podstawowy | ✅ zaimplementowane | CRUD |
| Płatności | ✅ zaimplementowane | tylko odczyt |
| Kategorie | ✅ zaimplementowane | tylko odczyt |
| Dokumenty magazynowe | ⬜ do zaimplementowania | PZ, WZ, MM |
| Faktury cykliczne | ⬜ do zaimplementowania | CRUD definicji cyklicznych |
| Działy (departments) | ⬜ do zaimplementowania | zarządzanie działami |
| Rachunki bankowe | ⬜ do zaimplementowania | wg API_RACHUNKI_BANKOWE.md |
| Integracje (zarządzanie kontami) | ⬜ do zaimplementowania | tworzenie kont przez integration_token |

---

## Referencje

- [Fakturownia API — dokumentacja GitHub](https://github.com/fakturownia/api)
- [Fakturownia API — strona](https://app.fakturownia.pl/api)
