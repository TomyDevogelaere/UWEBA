using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Globalization;

namespace PizzaPlace.Client.Shared;

public partial class CultureSelector
{
    [Inject]
    public IStringLocalizer<CultureSelector> Localizer { get; set; } = default!;
    [Inject]
    public NavigationManager NavManager { get; set; } = default!;
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    private List<string> cultures = new List<string>
    {
        "en-US", "nl-BE", "fr-BE"
    };

    

    public string Culture
    {
        get { return CultureInfo.DefaultThreadCurrentCulture?.Name??"nl-BE"; }
        set 
        {
            if (Culture != value)
            {
                ((IJSInProcessRuntime)JSRuntime).InvokeVoid("blazorCulture.set", value);
                //force the page to reload
                NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
            }
        }
    }

}
