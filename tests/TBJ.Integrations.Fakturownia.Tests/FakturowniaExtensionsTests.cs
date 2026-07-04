using TBJ.Integrations.Fakturownia.Configuration;
using TBJ.Integrations.Fakturownia.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TBJ.Integrations.Fakturownia.Tests;

/// <summary>
/// Testy rejestracji DI dla biblioteki Fakturownia.
/// </summary>
public class FakturowniaExtensionsTests
{
    [Fact]
    public void AddFakturownia_RejestrujeWszystkichKlientow()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        // Act
        services.AddFakturownia(opt =>
        {
            opt.Timeout = TimeSpan.FromSeconds(30);
        });

        var provider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(provider.GetService<IInvoicesClient>());
        Assert.NotNull(provider.GetService<IClientsClient>());
        Assert.NotNull(provider.GetService<IProductsClient>());
        Assert.NotNull(provider.GetService<IPaymentsClient>());
        Assert.NotNull(provider.GetService<ICategoriesClient>());
        Assert.NotNull(provider.GetService<IFakturowniaClient>());
    }

    [Fact]
    public void AddFakturownia_ZKonfiguracja_RejestrujeOpcje()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Fakturownia:Timeout"] = "00:00:45",
                ["Fakturownia:ApiToken"] = "token",
                ["Fakturownia:Domain"] = "myfirm"
            })
            .Build();

        // Act
        services.AddFakturownia(configuration);
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<FakturowniaOptions>();

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(45), options.Timeout);
        Assert.Equal("token", options.ApiToken);
        Assert.Equal("myfirm", options.Domain);
    }

    [Fact]
    public void AddFakturownia_NieprawidlowyTimeout_RzucaWyjatek()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => services.AddFakturownia(opt => opt.Timeout = TimeSpan.Zero));
    }
}
