using TBJ.Integrations.Fakturownia.Clients;
using TBJ.Integrations.Fakturownia.Configuration;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TBJ.Integrations.Fakturownia;

/// <summary>
/// Rozszerzenia rejestracji integracji z Fakturownia API w kontenerze DI.
/// </summary>
public static class FakturowniaExtensions
{
    /// <summary>
    /// Rejestruje integrację z Fakturownia API z konfiguracją inline.
    /// </summary>
    /// <param name="services">Kolekcja usług.</param>
    /// <param name="configure">Akcja konfiguracji opcji (Timeout, oraz opcjonalne fallback ApiToken i Domain dla Scenariusza B).</param>
    /// <returns>Kolekcja usług (fluent API).</returns>
    /// <remarks>
    /// <para>
    /// <b>Scenariusz A – konto tenanta:</b> nie podawaj ApiToken/Domain tutaj.
    /// Przekazuj <c>FakturowniaAuthInfo</c> per-żądanie w metodach klientów.
    /// </para>
    /// <para>
    /// <b>Scenariusz B – nasze konto:</b> ustaw <c>opt.ApiToken</c> i <c>opt.Domain</c>
    /// jako fallback używany gdy wywołujący nie przekazuje <c>FakturowniaAuthInfo</c>.
    /// </para>
    /// <example>
    /// <code>
    /// // Scenariusz A (tylko infrastruktura, credentials per-request):
    /// builder.Services.AddFakturownia();
    ///
    /// // Scenariusz B (nasze konto jako fallback):
    /// builder.Services.AddFakturownia(opt =>
    /// {
    ///     opt.ApiToken = "xxx-yyy-zzz";
    ///     opt.Domain = "myfirm";
    /// });
    ///
    /// // Oба scenariusze jednocześnie (Scenariusz B jako fallback):
    /// builder.Services.AddFakturownia(builder.Configuration);
    /// </code>
    /// </example>
    /// </remarks>
    public static IServiceCollection AddFakturownia(
        this IServiceCollection services,
        Action<FakturowniaOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new FakturowniaOptions();
        configure?.Invoke(options);
        ValidateOptions(options);

        services.AddSingleton(options);

        services.AddHttpClient<FakturowniaHttpClient>((_, client) =>
        {
            // BaseAddress nie jest ustawiony globalnie — URL budowany dynamicznie per-request
            // na podstawie Domain z FakturowniaAuthInfo lub FakturowniaOptions.Domain
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IInvoicesClient>(sp =>
            new InvoicesClient(sp.GetRequiredService<FakturowniaHttpClient>()));

        services.AddScoped<IClientsClient>(sp =>
            new ClientsClient(sp.GetRequiredService<FakturowniaHttpClient>()));

        services.AddScoped<IProductsClient>(sp =>
            new ProductsClient(sp.GetRequiredService<FakturowniaHttpClient>()));

        services.AddScoped<IPaymentsClient>(sp =>
            new PaymentsClient(sp.GetRequiredService<FakturowniaHttpClient>()));

        services.AddScoped<ICategoriesClient>(sp =>
            new CategoriesClient(sp.GetRequiredService<FakturowniaHttpClient>()));

        services.AddScoped<IFakturowniaClient>(sp =>
            new FakturowniaClient(
                sp.GetRequiredService<IInvoicesClient>(),
                sp.GetRequiredService<IClientsClient>(),
                sp.GetRequiredService<IProductsClient>(),
                sp.GetRequiredService<IPaymentsClient>(),
                sp.GetRequiredService<ICategoriesClient>()));

        return services;
    }

    /// <summary>
    /// Rejestruje integrację z Fakturownia API, odczytując konfigurację z sekcji
    /// <see cref="FakturowniaOptions.SectionName"/> w <c>appsettings.json</c>.
    /// </summary>
    /// <param name="services">Kolekcja usług.</param>
    /// <param name="configuration">Konfiguracja aplikacji.</param>
    /// <returns>Kolekcja usług (fluent API).</returns>
    public static IServiceCollection AddFakturownia(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(FakturowniaOptions.SectionName);

        return services.AddFakturownia(opt =>
        {
            var timeout = section["Timeout"];
            if (!string.IsNullOrWhiteSpace(timeout) && TimeSpan.TryParse(timeout, out var ts))
                opt.Timeout = ts;

            var apiToken = section["ApiToken"];
            if (!string.IsNullOrWhiteSpace(apiToken))
                opt.ApiToken = apiToken;

            var domain = section["Domain"];
            if (!string.IsNullOrWhiteSpace(domain))
                opt.Domain = domain;
        });
    }

    private static void ValidateOptions(FakturowniaOptions options)
    {
        if (options.Timeout <= TimeSpan.Zero)
            throw new InvalidOperationException(
                $"Wartość {nameof(FakturowniaOptions.Timeout)} musi być większa od zera.");
    }
}
