using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Messaging.WhatsApp.Responses;

namespace WAChatFlow.Server.Services.Messaging.Senders
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
