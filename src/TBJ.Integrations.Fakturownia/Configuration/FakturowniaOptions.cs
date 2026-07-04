namespace TBJ.Integrations.Fakturownia.Configuration;

/// <summary>
/// Opcje infrastrukturalne integracji z Fakturownia.pl — wspólne dla wszystkich tenantów.
/// </summary>
/// <remarks>
/// <para>
/// Biblioteka obsługuje dwa scenariusze uwierzytelniania:
/// </para>
/// <list type="bullet">
///   <item>
///     <description>
///       <b>Scenariusz A – konto tenanta:</b> tenant posiada własne konto Fakturownia.
///       Credentials (<c>ApiToken</c>, <c>Domain</c>) przechowywane są w bazie danych
///       aplikacji nadrzędnej i przekazywane per-żądanie przez <see cref="Auth.FakturowniaAuthInfo"/>.
///     </description>
///   </item>
///   <item>
///     <description>
///       <b>Scenariusz B – nasze konto:</b> chcemy wyświetlić tenantowi faktury wystawione przez nas.
///       Credentials (<see cref="ApiToken"/>, <see cref="Domain"/>) ładowane są z <c>appsettings.json</c>
///       (niniejsza klasa) i używane automatycznie gdy <see cref="Auth.FakturowniaAuthInfo"/> nie jest przekazane.
///     </description>
///   </item>
/// </list>
/// </remarks>
public sealed class FakturowniaOptions
{
    /// <summary>
    /// Nazwa sekcji konfiguracyjnej w <c>appsettings.json</c>.
    /// </summary>
    public const string SectionName = "Fakturownia";

    /// <summary>
    /// Timeout dla żądań HTTP do Fakturownia API.
    /// Domyślnie: 30 sekund.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Fallback API token (Scenariusz B – nasze konto).
    /// Używany gdy <see cref="Auth.FakturowniaAuthInfo"/> nie jest przekazane w wywołaniu.
    /// Wartość z ustawień konta: Ustawienia → Ustawienia konta → Integracja → Kod autoryzacyjny API.
    /// </summary>
    public string? ApiToken { get; set; }

    /// <summary>
    /// Fallback subdomena konta Fakturownia (Scenariusz B – nasze konto).
    /// Np. dla <c>https://myfirm.fakturownia.pl</c> wartość to <c>myfirm</c>.
    /// Używana gdy <see cref="Auth.FakturowniaAuthInfo"/> nie jest przekazane w wywołaniu.
    /// </summary>
    public string? Domain { get; set; }
}
