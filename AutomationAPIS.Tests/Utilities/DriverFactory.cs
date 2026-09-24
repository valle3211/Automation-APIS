using Microsoft.Playwright;

namespace AutomationAPIS.Tests.Utilities;

public static class FabricaDeNavegador
{
    public static async Task<IPlaywright> CrearPlaywrightAsync()
        => await Playwright.CreateAsync();

    public static async Task<IBrowser> CrearNavegadorAsync(IPlaywright playwright)
    {
        var configuracion = LectorConfiguracion.Configuracion;
        var opcionesInicio = new BrowserTypeLaunchOptions { Headless = configuracion.SinInterfaz };

        return configuracion.Navegador.ToLowerInvariant() switch
        {
            "firefox" => await playwright.Firefox.LaunchAsync(opcionesInicio),
            "webkit" => await playwright.Webkit.LaunchAsync(opcionesInicio),
            _ => await playwright.Chromium.LaunchAsync(opcionesInicio),
        };
    }
}
