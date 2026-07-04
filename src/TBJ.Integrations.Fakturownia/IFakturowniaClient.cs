using TBJ.Integrations.Fakturownia.Interfaces;

namespace TBJ.Integrations.Fakturownia;

/// <summary>
/// Główny klient integracji z Fakturownia API.
/// Agreguje klientów poszczególnych obszarów API.
/// </summary>
/// <remarks>
/// Rejestruj w DI przez <c>AddFakturownia()</c>.
/// <para>
/// Każdy obszar API obsługuje dwa scenariusze uwierzytelniania:
/// </para>
/// <list type="bullet">
///   <item>
///     <description>
///       <b>Scenariusz A – konto tenanta:</b> przekaż <c>FakturowniaAuthInfo</c> w wywołaniu metody.
///     </description>
///   </item>
///   <item>
///     <description>
///       <b>Scenariusz B – nasze konto:</b> przekaż <c>null</c> lub pomiń parametr — 
///       credentials ładowane są z <c>appsettings.json</c> (sekcja "Fakturownia").
///     </description>
///   </item>
/// </list>
/// </remarks>
public interface IFakturowniaClient
{
    /// <summary>Klient do zarządzania fakturami.</summary>
    IInvoicesClient Invoices { get; }

    /// <summary>Klient do zarządzania kontrahentami (klientami).</summary>
    IClientsClient Clients { get; }

    /// <summary>Klient do zarządzania produktami / usługami.</summary>
    IProductsClient Products { get; }

    /// <summary>Klient do zarządzania płatnościami.</summary>
    IPaymentsClient Payments { get; }

    /// <summary>Klient do zarządzania kategoriami.</summary>
    ICategoriesClient Categories { get; }
}
