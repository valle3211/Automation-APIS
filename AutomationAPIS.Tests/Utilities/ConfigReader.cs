using System.Text.Json;

namespace AutomationAPIS.Tests.Utilities;

public static class LectorConfiguracion
{
    private static readonly Lazy<ConfiguracionApp> _configuracion = new(Cargar);

    public static ConfiguracionApp Configuracion => _configuracion.Value;

    private static ConfiguracionApp Cargar()
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var json = File.ReadAllText(ruta);
        return JsonSerializer.Deserialize<ConfiguracionApp>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("No se pudo leer appsettings.json");
    }
}

public class ConfiguracionApp
{
    public string UrlBase { get; set; } = string.Empty;
    public string UrlBaseApi { get; set; } = string.Empty;
    public string Navegador { get; set; } = "chromium";
    public bool SinInterfaz { get; set; } = true;
}
