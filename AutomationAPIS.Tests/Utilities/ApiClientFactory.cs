using Microsoft.Playwright;

namespace AutomationAPIS.Tests.Utilities;

public static class FabricaClienteApi
{
    public static async Task<IAPIRequestContext> CrearAsync(IPlaywright playwright, string urlBase)
        => await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = urlBase
        });
}
