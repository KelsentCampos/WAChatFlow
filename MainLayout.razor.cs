using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace WAChatFlow.Client.Components.Layout
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject] private NavigationManager Nav { get; set; } = default!;

        protected override void OnInitialized()
            => Nav.LocationChanged += OnChanged;

        private void OnChanged(object? s, LocationChangedEventArgs e) => StateHasChanged();

        public void Dispose() => Nav.LocationChanged -= OnChanged;
    }
}
