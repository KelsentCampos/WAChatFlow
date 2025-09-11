using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace WAChatFlow.Client.Pages.UserLogs
{
    public class LogsBase : ComponentBase
    {
        protected List<LogModel> Logs { get; set; } = new()
        {
            new() { FechaEnvio=DateTime.Now.AddMinutes(-30), Destinatario="+52 55 1234 5678", TipoMensaje="Plantilla", Estado="Entregado", IdMensaje="wamid.HBgMM..." },
            new() { FechaEnvio=DateTime.Now.AddMinutes(-20), Destinatario="+52 55 8765 4321", TipoMensaje="Texto", Estado="Fallido", IdMensaje="wamid.HBgMM..." },
            new() { FechaEnvio=DateTime.Now.AddMinutes(-10), Destinatario="+52 55 1122 3344", TipoMensaje="Imagen", Estado="Pendiente", IdMensaje="wamid.HBgMM..." }
        };

        protected string GetBadgeClass(string estado) => estado switch
        {
            "Entregado" => "badge-success",
            "Fallido" => "badge-danger",
            "Pendiente" => "badge-warning",
            _ => "badge-secondary"
        };
    }

    public class LogModel
    {
        public DateTime FechaEnvio { get; set; }
        public string Destinatario { get; set; } = string.Empty;
        public string TipoMensaje { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string IdMensaje { get; set; } = string.Empty;
    }
}
