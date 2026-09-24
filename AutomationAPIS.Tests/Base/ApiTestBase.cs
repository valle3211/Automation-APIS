using Microsoft.Playwright;
using AutomationAPIS.Tests.Utilities;

namespace AutomationAPIS.Tests.Base;

public abstract class PruebaBaseApi
{
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IAPIRequestContext ContextoApi { get; private set; } = null!;

    private readonly List<RegistroLlamadaApi> _llamadas = new();
    private DateTime _horaInicio;

    [SetUp]
    public async Task ConfiguracionInicial()
    {
        _horaInicio = DateTime.Now;
        Playwright = await FabricaDeNavegador.CrearPlaywrightAsync();
        ContextoApi = await FabricaClienteApi.CrearAsync(Playwright, LectorConfiguracion.Configuracion.UrlBaseApi);
        _llamadas.Clear();
    }

    // Ejecuta el GET y guarda el detalle (status + body + hora) para el reporte HTML
    protected async Task<(IAPIResponse Respuesta, string Cuerpo)> ObtenerAsync(string ruta)
    {
        var respuesta = await ContextoApi.GetAsync(ruta);
        var cuerpo = await respuesta.TextAsync();
        _llamadas.Add(new RegistroLlamadaApi(DateTime.Now, "GET", ruta, respuesta.Status, cuerpo));
        return (respuesta, cuerpo);
    }

    // Ejecuta el POST con los datos dados y guarda el detalle para el reporte HTML
    protected async Task<(IAPIResponse Respuesta, string Cuerpo)> CrearAsync(string ruta, object datos)
    {
        var respuesta = await ContextoApi.PostAsync(ruta, new APIRequestContextOptions { DataObject = datos });
        var cuerpo = await respuesta.TextAsync();
        _llamadas.Add(new RegistroLlamadaApi(DateTime.Now, "POST", ruta, respuesta.Status, cuerpo));
        return (respuesta, cuerpo);
    }

    // Ejecuta el PUT con los datos dados y guarda el detalle para el reporte HTML
    protected async Task<(IAPIResponse Respuesta, string Cuerpo)> ActualizarAsync(string ruta, object datos)
    {
        var respuesta = await ContextoApi.PutAsync(ruta, new APIRequestContextOptions { DataObject = datos });
        var cuerpo = await respuesta.TextAsync();
        _llamadas.Add(new RegistroLlamadaApi(DateTime.Now, "PUT", ruta, respuesta.Status, cuerpo));
        return (respuesta, cuerpo);
    }

    [TearDown]
    public async Task LimpiezaFinal()
    {
        var horaFin = DateTime.Now;
        var resultado = TestContext.CurrentContext.Result;
        ConstructorReporteHtml.AgregarResultadoPrueba(new EntradaReportePrueba
        {
            NombrePrueba = TestContext.CurrentContext.Test.FullName,
            Resultado = resultado.Outcome.Status.ToString(),
            MensajeError = resultado.Message,
            HoraInicio = _horaInicio,
            HoraFin = horaFin,
            DuracionMs = (horaFin - _horaInicio).TotalMilliseconds,
            Etiquetas = TestContext.CurrentContext.Test.Properties["Category"].Select(c => c!.ToString()!).ToList(),
            Llamadas = new List<RegistroLlamadaApi>(_llamadas)
        });

        await ContextoApi.DisposeAsync();
        Playwright.Dispose();
    }
}
