using static WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs.TemplateMappingsDto;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace WAChatFlow.Client.Pages
{
    public class TemplatesBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; } = null!;
        [Inject] protected NavigationManager navigationManager { get; set; } = default!;
        protected List<TemplateDetailDto> templates { get; set; } = new List<TemplateDetailDto>();

        protected override async Task OnInitializedAsync()
        {
            await LoadTemplatesAsync();
        }

        protected async Task LoadTemplatesAsync()
        {
            var resp = await Http.GetAsync("api/templates/list-templates-with-details"); 

            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadFromJsonAsync<List<TemplateDetailDto>>();

                templates = data ?? new List<TemplateDetailDto>();
            }
            else
            {
                Console.WriteLine("Error al cargar las plantillas.");
            }

            await Task.Delay(500);
        }

        protected string GetBadgeClass(string status)
        {
            if (string.Equals(status, "APPROVED", StringComparison.OrdinalIgnoreCase)) return "badge-success";
            if (string.Equals(status, "PENDING", StringComparison.OrdinalIgnoreCase)) return "badge-warning";
            if (string.Equals(status, "REJECTED", StringComparison.OrdinalIgnoreCase)) return "badge-danger";
            return "badge-secondary";
        }

        protected async Task UsarPlantilla(string templateId)
        {
            navigationManager.NavigateTo($"/use-template/{Uri.EscapeDataString(templateId)}");
            await Task.CompletedTask;
        }

        protected async Task CrearNuevaPlantilla()
        {
            Console.WriteLine("Creando nueva plantilla...");
        }
    }
}