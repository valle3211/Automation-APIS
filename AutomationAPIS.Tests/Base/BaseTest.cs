using Microsoft.Playwright;
using AutomationAPIS.Tests.Utilities;

namespace AutomationAPIS.Tests.Base;

public abstract class PruebaBase
{
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IBrowser Navegador { get; private set; } = null!;
    protected IPage Pagina { get; private set; } = null!;

    [SetUp]
    public async Task ConfiguracionInicial()
    {
        Playwright = await FabricaDeNavegador.CrearPlaywrightAsync();
        Navegador = await FabricaDeNavegador.CrearNavegadorAsync(Playwright);
        Pagina = await Navegador.NewPageAsync();
    }

    [TearDown]
    public async Task LimpiezaFinal()
    {
        await Pagina.CloseAsync();
        await Navegador.CloseAsync();
        Playwright.Dispose();
    }
}
