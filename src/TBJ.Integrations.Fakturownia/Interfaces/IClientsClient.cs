using TBJ.Integrations.Fakturownia.Auth;
using TBJ.Integrations.Fakturownia.Models.Clients;

namespace TBJ.Integrations.Fakturownia.Interfaces;

/// <summary>
/// Klient do zarządzania kontrahentami (klientami) w Fakturownia API.
/// </summary>
/// <remarks>
/// Wszystkie metody akceptują opcjonalny parametr <see cref="FakturowniaAuthInfo"/>.
/// Gdy <c>auth</c> jest <c>null</c>, używane są credentials z konfiguracji appsettings.
/// </remarks>
public interface IClientsClient
{
    /// <summary>
    /// Pobiera listę klientów. Opcjonalnie filtruje po nazwie lub NIP.
    /// </summary>
    Task<IReadOnlyList<Client>> GetClientsAsync(
        string? name = null,
        string? taxNo = null,
        int page = 1,
        int perPage = 25,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Pobiera pojedynczego klienta po identyfikatorze.
    /// </summary>
    Task<Client> GetClientAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Tworzy nowego klienta.
    /// </summary>
    Task<Client> CreateClientAsync(
        CreateClientRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Aktualizuje dane istniejącego klienta.
    /// </summary>
    Task<Client> UpdateClientAsync(
        long id,
        UpdateClientRequest request,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);

    /// <summary>
    /// Usuwa klienta.
    /// </summary>
    Task DeleteClientAsync(
        long id,
        FakturowniaAuthInfo? auth = null,
        CancellationToken ct = default);
}
