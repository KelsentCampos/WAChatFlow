using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System;

namespace WAChatFlow.Client.Shared.Navigation
{
    public class NavMenuBase : ComponentBase, IAsyncDisposable
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private IJSObjectReference? _module;
        public record NavItem(string Href, string Text, string Icon, NavLinkMatch Match);

        protected NavItem[] Items = new[]
        {
        new NavItem("/",                "Usuarios",         "👥", NavLinkMatch.All),
        new NavItem("messages",         "Enviar Mensajes",  "✉️", NavLinkMatch.Prefix),
        new NavItem("templates",        "Plantillas",       "📄", NavLinkMatch.Prefix),
        new NavItem("activity-log",     "Logs",             "📊", NavLinkMatch.Prefix), // este es para configuracion "⚙"
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Shared/Navigation/NavMenu.razor.js");
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