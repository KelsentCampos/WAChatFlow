using WAChatFlow.Shared.Messaging.WhatsApp.Responses;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;

namespace WAChatFlow.Server.Services.Templates
{
    public interface ITemplateMappings
    {
        TemplateMapDto.TemplateListResponseDto ToListDto(WhatsAppTemplatesResponse whatsAppTemplatesResponse);
        TemplateMapDto.TemplateDetailDto ToDetailDto(MessageTemplate template);
        List<TemplateMapDto.TemplateDetailDto> ToDetailListDto(WhatsAppTemplatesResponse templatesResponse);
    }
}
