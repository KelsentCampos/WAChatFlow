using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace WAChatFlow.Client.Components.Layout
{
    public class NavMenuBase : ComponentBase, IAsyncDisposable
    {
        public record NavItem(string Href, string Text, string Icon, NavLinkMatch Match);

        protected NavItem[] Items = new[]
        {
        new NavItem("/",            "Usuarios",         "👥", NavLinkMatch.All),
        new NavItem("send-messages","Enviar Mensajes",  "✉️", NavLinkMatch.Prefix),
        new NavItem("templates",    "Plantillas",       "📄", NavLinkMatch.Prefix),
        new NavItem("logs",         "Logs",             "📊", NavLinkMatch.Prefix), // este es para configuracion "⚙"
    };

        [Inject] private IJSRuntime JS { get; set; } = default!;
        private IJSObjectReference? _module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Layout/NavMenu.razor.js");
            }
        }

        protected async Task CloseMenu()
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("closeMenuIfMobile", "navCheck");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
            }
        }
    }
}