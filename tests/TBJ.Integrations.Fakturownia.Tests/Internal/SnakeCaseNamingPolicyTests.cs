using System.Text.Json;
using TBJ.Integrations.Fakturownia.Internal;

namespace TBJ.Integrations.Fakturownia.Tests.Internal;

/// <summary>
/// Testy polityki nazewnictwa snake_case używanej do serializacji JSON w Fakturownia.
/// </summary>
public class SnakeCaseNamingPolicyTests
{
    [Fact]
    public void JsonOptions_SnakeCaseLower_NazwyWlasciwosciSaSnakeCase()
    {
        // Arrange
        var options = FakturowniaHttpClient.JsonOptions;
        var json = JsonSerializer.Serialize(new TestModel { InvoiceNumber = "FV/1" }, options);

        // Assert
        Assert.Contains("\"invoice_number\"", json);
    }

    private sealed class TestModel
    {
        public string InvoiceNumber { get; set; } = string.Empty;
    }
}
