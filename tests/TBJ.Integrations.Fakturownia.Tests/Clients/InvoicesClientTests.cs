using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using TBJ.Integrations.Fakturownia.Clients;
using TBJ.Integrations.Fakturownia.Configuration;
using TBJ.Integrations.Fakturownia.Internal;
using TBJ.Integrations.Fakturownia.Models.Invoices;

namespace TBJ.Integrations.Fakturownia.Tests.Clients;

/// <summary>
/// Testy klienta faktur — kształt URL wysyłanego do Fakturownia API,
/// w szczególności flaga <c>include_positions</c> sterowana parametrem metody.
/// </summary>
public class InvoicesClientTests
{
    [Fact]
    public async Task GetInvoicesAsync_IncludePositionsTrue_DodajeParametrDoUrl()
    {
        // Arrange
        var (client, handler) = CreateClient();

        // Act
        await client.GetInvoicesAsync(new InvoiceListFilter { Page = 1 }, includePositions: true);

        // Assert
        Assert.Contains("include_positions=true", handler.LastRequestUri!.Query);
    }

    [Fact]
    public async Task GetInvoicesAsync_Domyslnie_BrakIncludePositionsWUrl()
    {
        // Arrange
        var (client, handler) = CreateClient();

        // Act
        await client.GetInvoicesAsync(new InvoiceListFilter { Page = 1 });

        // Assert
        Assert.DoesNotContain("include_positions", handler.LastRequestUri!.Query);
    }

    /// <summary>Buduje klienta na podrzuconym handlerze HTTP zwracającym pustą listę.</summary>
    private static (InvoicesClient Client, CapturingHandler Handler) CreateClient()
    {
        var handler = new CapturingHandler();
        var http = new HttpClient(handler);
        var options = new FakturowniaOptions { Domain = "test", ApiToken = "token" };
        var httpClient = new FakturowniaHttpClient(http, options, NullLogger<FakturowniaHttpClient>.Instance);

        return (new InvoicesClient(httpClient), handler);
    }

    /// <summary>Handler zapamiętujący URL żądania i zwracający pustą listę JSON.</summary>
    private sealed class CapturingHandler : HttpMessageHandler
    {
        public Uri? LastRequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]", System.Text.Encoding.UTF8, "application/json")
            });
        }
    }
}
