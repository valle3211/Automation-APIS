using AutomationAPIS.Tests.Utilities;

namespace AutomationAPIS.Tests;

[SetUpFixture]
public class GanchosGlobalesPruebas
{
    [OneTimeTearDown]
    public void GenerarReporteHtml()
    {
        var carpetaReportes = Path.Combine(EncontrarRaizProyecto(), "Reportes");
        var rutaSalida = Path.Combine(carpetaReportes, "TestReport.html");
        ConstructorReporteHtml.GenerarReporte(rutaSalida);
        TestContext.Progress.WriteLine($"Reporte HTML generado en: {rutaSalida}");
    }
    private static string EncontrarRaizProyecto()
    {
        var directorio = new DirectoryInfo(AppContext.BaseDirectory);
        while (directorio is not null && directorio.GetFiles("*.csproj").Length == 0)
        {
            directorio = directorio.Parent;
        }

        return directorio?.FullName ?? AppContext.BaseDirectory;
    }
}
