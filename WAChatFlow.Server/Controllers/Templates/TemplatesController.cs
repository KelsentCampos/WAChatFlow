using Microsoft.AspNetCore.Mvc;
using WAChatFlow.Server.Services.Messaging.Builders;
using WAChatFlow.Server.Services.Messaging.Senders;
using WAChatFlow.Server.Services.Templates;

namespace WAChatFlow.Server.Controllers.Templates
{
    [ApiController]
    [Route("api/templates")]
    public class TemplatesController : ControllerBase
    {
        private readonly IWhatsAppMessageSender _whatsAppMessageSender;
        private readonly IWhatsAppRequestBuilder _whatsAppRequestBuilder;
        private readonly ITemplateMappings _templateMappings;

        public TemplatesController(IWhatsAppMessageSender whatsAppMessage, IWhatsAppRequestBuilder whatsAppRequest, ITemplateMappings templateMappings)
        {
            _whatsAppMessageSender = whatsAppMessage;
            _whatsAppRequestBuilder = whatsAppRequest;
            _templateMappings = templateMappings;
        }

        [HttpGet("list-templates")]
        public async Task<IActionResult> GetTemplates()
        {
            try
            {
                var metaResult = await _whatsAppMessageSender.GetTemplatesAsync();

                var dtoTemplate = _templateMappings.ToListDto(metaResult);

                return Ok(dtoTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(502, new { success = false, message = "No se pudieron obtener las plantillas.", detail = ex.Message });
            }
        }

        [HttpGet("list-templates-with-details")]
        public async Task<IActionResult> GetTemplatesWithDetails()
        {
            try
            {
                var metaResult = await _whatsAppMessageSender.GetTemplatesAsync();

                var items = _templateMappings.ToDetailListDto(metaResult);

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(502, new { success = false, message = "No se pudieron obtener las plantillas.", detail = ex.Message });
            }
        }

        [HttpGet("template-details/{templateId}")]
        public async Task<IActionResult> GetTemplateDetails(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId))
            {
                return BadRequest(new { success = false, message = "El ID de la plantilla es requerido." });
            }

            try
            {
                var metaResult = await _whatsAppMessageSender.GetTemplateIdResponseAsyn(templateId);

                var dto = _templateMappings.ToDetailDto(metaResult);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(502, new { success = false, message = "No se pudieron obtener los detalles de la plantilla.", detail = ex.Message });
            }
        }
    }
}