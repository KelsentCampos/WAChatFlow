using WAChatFlow.Shared.Messaging.WhatsApp.Responses;
using static WAChatFlow.Shared.Messaging.WhatsApp.Requests.TemplateMapDto;

namespace WAChatFlow.Server.Services.Templates
{
    public class TemplateMappings : ITemplateMappings
    {
        public TemplateListResponseDto ToListDto(WhatsAppTemplatesResponse templatesResponse)
        {
            TemplateListResponseDto responseDto = new TemplateListResponseDto();

            if (templatesResponse is not null && templatesResponse.Data is not null)
            {
                for (int i = 0; i < templatesResponse.Data.Count; i++)
                { 
                    var dataItem = templatesResponse.Data[i];

                    TemplateListItemDto itemDto;

                    itemDto = new TemplateListItemDto
                    {
                        Id = dataItem?.Id ?? string.Empty,
                        Name = dataItem?.Name ?? string.Empty,
                        Language = dataItem?.Language ?? string.Empty,
                        Status = dataItem?.Status ?? string.Empty,
                        Category = dataItem?.Category ?? string.Empty,
                        ComponentsCount = dataItem?.Components != null ? dataItem.Components.Count : 0
                    };

                    bool hasBody = false;
                    bool hasButtons = false;

                    if (dataItem?.Components is not null)
                    {
                        for (int j = 0; j < dataItem.Components.Count; j++)
                        {
                            var component = dataItem.Components[j];

                            if (!hasBody && string.Equals(component?.Type, "BODY", StringComparison.OrdinalIgnoreCase)) hasBody = true;
                            if (!hasButtons && string.Equals(component?.Type, "BUTTONS", StringComparison.OrdinalIgnoreCase)) hasButtons = true;
                            if (hasBody && hasButtons) break;
                        }
                    }

                    itemDto.HasBody = hasBody;
                    itemDto.HasButtons = hasButtons;

                    responseDto.Items.Add(itemDto);
                }
            }

            if (templatesResponse?.Paging?.Cursors != null)
            {
                responseDto.Before = templatesResponse.Paging.Cursors.Before;
                responseDto.After = templatesResponse.Paging.Cursors.After;
            }

            return responseDto;
        }

        public TemplateDetailDto ToDetailDto(MessageTemplate template)
        {
            TemplateDetailDto detailDto = new TemplateDetailDto();

            if (template is not null)
            {
                detailDto = new TemplateDetailDto
                {
                    Id = template.Id,
                    Name = template.Name,
                    Language = template.Language,
                    Status = template.Status,
                    Category = template.Category,
                    SubCategory = template.SubCategory,
                    ParameterFormat = template.ParameterFormat,
                    MessageSendTtlSeconds = template.MessageSendTtlSeconds,
                    PreviousCategory = template.PreviousCategory,
                    Components = new List<TemplateComponentDto>()
                };

                if (template?.Components != null)
                { 
                    for (int i = 0; i < template.Components.Count; i++)
                    {

                        TemplateComponentDto componentDto = new TemplateComponentDto();

                        var templateComponent = template.Components[i];

                        componentDto = new TemplateComponentDto
                        {
                            Type = templateComponent?.Type ?? string.Empty,
                            Format = templateComponent?.Format,
                            Text = templateComponent?.Text,
                            AddSecurityRecommendation = templateComponent?.AddSecurityRecommendation,
                            CodeExpirationMinutes = templateComponent?.CodeExpirationMinutes,
                            BodyExamples = templateComponent?.Example?.BodyText
                        };

                        if (templateComponent?.Buttons != null && templateComponent.Buttons.Count > 0)
                        {
                            componentDto.Buttons = new List<TemplateButtonDto>();

                            for (int k = 0; k < templateComponent.Buttons.Count; k++)
                            {
                                TemplateButtonDto buttonDto1 = new TemplateButtonDto();

                                var templateButton = templateComponent.Buttons[k];

                                buttonDto1 = new TemplateButtonDto
                                {
                                    Type = templateButton?.Type ?? string.Empty,
                                    Text = templateButton?.Text,
                                    Url = templateButton?.Url,
                                    PhoneNumber = templateButton?.PhoneNumber,
                                    Example = templateButton?.Example
                                };
                                
                                componentDto.Buttons.Add(buttonDto1);
                            }
                        }
                        
                        detailDto.Components.Add(componentDto);
                    }
                }
            }

            return detailDto;
        }

        public List<TemplateDetailDto> ToDetailListDto(WhatsAppTemplatesResponse templatesResponse)
        {
            List<TemplateDetailDto> detailList = new List<TemplateDetailDto>();

            if (templatesResponse is not null && templatesResponse.Data is not null)
            {
                for (int i = 0; i < templatesResponse.Data.Count; i++)
                {
                    var dataItem = templatesResponse.Data[i];

                    if (dataItem is not null)
                    {
                        var detailDto = ToDetailDto(dataItem);
                        detailList.Add(detailDto);
                    }
                }
            }
            return detailList;
        }
    }
}
