namespace TBJ.Integrations.Fakturownia.Auth;

/// <summary>
/// Dane uwierzytelniające do Fakturownia API dla konkretnego tenanta.
/// </summary>
/// <remarks>
/// Dane te są specyficzne dla tenanta i powinny być przechowywane w bazie danych
/// aplikacji nadrzędnej — nie w <c>appsettings.json</c>.
/// <para>
/// Jeśli tenant posiada własne konto Fakturownia (Scenariusz A), przekaż ten obiekt
/// w każdym wywołaniu metod klientów API.
/// </para>
/// <para>
/// Jeśli chcesz operować na własnym koncie platformy (Scenariusz B — np. wystawione przez
/// Ciebie faktury dla tenanta), przekaż <c>null</c> — biblioteka użyje credentials
/// z <c>appsettings.json</c> (<see cref="Configuration.FakturowniaOptions"/>).
/// </para>
/// </remarks>
public sealed class FakturowniaAuthInfo
{
    /// <summary>
    /// Token API konta Fakturownia.
    /// Wartość z ustawień konta: Ustawienia → Ustawienia konta → Integracja → Kod autoryzacyjny API.
    /// </summary>
    public required string ApiToken { get; init; }

    /// <summary>
    /// Subdomena konta Fakturownia (bez protokołu i domeny głównej).
    /// Np. dla <c>https://myfirm.fakturownia.pl</c> wartość to <c>myfirm</c>.
    /// </summary>
    public required string Domain { get; init; }
}
