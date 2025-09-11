using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Models.WhatsApp.Responses.Templates;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs;


namespace WAChatFlow.Shared.Interfaces.WhatsApp
{
    public interface IWhatsAppMessageSender
    {
        Task<bool> ExecuteAsync(object model);
        Task<WhatsAppResponse> ExecuteWithResultAsync(object model);
        Task<MetaSendResult<WhatsAppResponse>> ExecuteDetailedAsync(object model);
        
        Task<WhatsAppTemplatesResponse> GetTemplatesAsync();
        Task<MessageTemplate> GetTemplateIdResponseAsyn(string id);
    }
}
