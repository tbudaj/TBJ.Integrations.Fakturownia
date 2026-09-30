using System.Text.Json;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Invoices;

namespace TBJ.Integrations.Fakturownia.Tests.Models;

/// <summary>
/// Testy deserializacji modelu <see cref="Invoice"/> z JSON zwracanego przez Fakturownia API.
/// </summary>
public class InvoiceDeserializationTests
{
    [Fact]
    public void Deserialize_PolaPlatnosci_MapowanePoprawnie()
    {
        // Arrange
        const string json = """
            {
              "id": 1,
              "status": "issued",
              "paid": "0,00",
              "paid_date": "",
              "token": "abc",
              "payment_url": "https://ezwm.fakturownia.pl/p/xyz",
              "price_gross": "123.45"
            }
            """;

        // Act
        var invoice = JsonSerializer.Deserialize<Invoice>(json, FakturowniaHttpClient.JsonOptions);

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(0m, invoice.Paid);
        Assert.Equal("", invoice.PaidDate);
        Assert.Equal("abc", invoice.Token);
        Assert.Equal("https://ezwm.fakturownia.pl/p/xyz", invoice.PaymentUrl);
        Assert.Equal(123.45m, invoice.PriceGross);
    }

    [Theory]
    [InlineData("\"12.50\"", 12.5)]
    [InlineData("\"12,50\"", 12.5)]
    [InlineData("12.5", 12.5)]
    public void Deserialize_PaidStringLubLiczba_ParsujeNaDecimal(string paidJson, decimal expected)
    {
        // Arrange
        var json = $$"""{"id":1,"paid":{{paidJson}}}""";

        // Act
        var invoice = JsonSerializer.Deserialize<Invoice>(json, FakturowniaHttpClient.JsonOptions);

        // Assert
        Assert.Equal(expected, invoice!.Paid);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("\"\"")]
    public void Deserialize_PaidNullLubPusty_ParsujeNaNull(string paidJson)
    {
        // Arrange
        var json = $$"""{"id":1,"paid":{{paidJson}}}""";

        // Act
        var invoice = JsonSerializer.Deserialize<Invoice>(json, FakturowniaHttpClient.JsonOptions);

        // Assert
        Assert.Null(invoice!.Paid);
    }
}
