using WAChatFlow.Shared.Models.WhatsApp.Responses.Templates;
using WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs;
using static WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs.TemplateMappingsDto;

namespace WAChatFlow.Shared.Interfaces.WhatsApp
{
    public interface ITemplateMappings
    {
        TemplateMappingsDto.TemplateListResponseDto ToListDto(WhatsAppTemplatesResponse whatsAppTemplatesResponse);
        TemplateMappingsDto.TemplateDetailDto ToDetailDto(MessageTemplate template);
        List<TemplateDetailDto> ToDetailListDto(WhatsAppTemplatesResponse templatesResponse);
    }
}
