using TBJ.Integrations.Fakturownia.Models.Invoices;

namespace TBJ.Integrations.Fakturownia.Tests.Models;

/// <summary>
/// Testy mapowania filtrów listy faktur na parametry zapytania.
/// </summary>
public class InvoiceListFilterTests
{
    [Fact]
    public void ToQueryParams_OrderUstawiony_DodajeParametrOrder()
    {
        // Arrange
        var filter = new InvoiceListFilter { Order = "issue_date.desc" };

        // Act
        var dict = filter.ToQueryParams();

        // Assert
        Assert.Equal("issue_date.desc", dict["order"]);
    }

    [Fact]
    public void ToQueryParams_OrderNull_BrakParametruOrder()
    {
        // Arrange
        var filter = new InvoiceListFilter();

        // Act
        var dict = filter.ToQueryParams();

        // Assert
        Assert.False(dict.ContainsKey("order"));
    }

}
