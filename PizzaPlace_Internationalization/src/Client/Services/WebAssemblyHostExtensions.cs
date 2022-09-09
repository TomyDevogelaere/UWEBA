using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;

namespace PizzaPlace.Client.Services;

public static class WebAssemblyHostExtensions
{
    private const string DefaultCultureName = "en-US";

    public static async Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
        string result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
        result ??= CultureInfo.DefaultThreadCurrentCulture?.Name ?? DefaultCultureName;
        var culture = new CultureInfo(result);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        await jsInterop.InvokeAsync<string>("blazorCulture.set", result);
    }
}
