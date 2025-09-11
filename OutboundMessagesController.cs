using Microsoft.AspNetCore.Mvc;
using System.Net;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Interfaces.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp.Error;

namespace WAChatFlow.Server.Controllers
{
    [ApiController]
    [Route("api/outbound-messages")]
    public class OutboundMessagesController : ControllerBase
    {
        private readonly IWhatsAppMessageSender _whatsAppMessageSender;
        private readonly IWhatsAppRequestBuilder _whatsAppRequestBuilder;
        public OutboundMessagesController(IWhatsAppMessageSender whatsAppMessageSender, IWhatsAppRequestBuilder whatsAppRequest)
        {
            _whatsAppMessageSender = whatsAppMessageSender;
            _whatsAppRequestBuilder = whatsAppRequest;
        }

        [HttpPost("send-text-message")]
        public async Task<IActionResult> SendTextMessage([FromBody] TextMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Body))
            {
                return BadRequest(new MetaSendResult<WhatsAppResponse>()
                { 
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = new WhatsAppErrorResponse
                    {
                        Error = new WhatsAppErrorResponse.ErrorBody { Message = "Número y cuerpo del mensaje son requeridos." }
                    }
                });
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.TypedTextMessage(dto)
                    : _whatsAppRequestBuilder.TypedTextMessageReply(dto);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MetaSendResult<WhatsAppResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Error = new WhatsAppErrorResponse
                    {
                        Error = new WhatsAppErrorResponse.ErrorBody 
                        { 
                            Message = "Error al enviar el mensaje.",
                            ErrorData = new WhatsAppErrorResponse.ErrorData
                            {
                                Details = ex.Message
                            }
                        }
                    }
                });
            }
        }

        [HttpPost("reaction-message")]
        public async Task<IActionResult> ReactionMessage([FromBody] ReactionReplyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.Emoji is null || string.IsNullOrWhiteSpace(dto.ReactToMessageId))
            {
                return BadRequest("Número, emoji y mensaje a reaccionar son requeridos.");
            }

            try
            {
                object message = _whatsAppRequestBuilder.ReactionReply(dto.PhoneNumber, dto.ReactToMessageId, dto.Emoji);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-image-message")]
        public async Task<IActionResult> SendImageMessage([FromBody] ImageMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Url))
            {
                return BadRequest("Número y URL de la imagen son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.ImageByUrl(dto.PhoneNumber, dto.Url, dto.Caption)
                    : _whatsAppRequestBuilder.ImageReplyByUrl(dto.PhoneNumber, dto.Url, dto.ReplyToMessageId, dto.Caption);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-audio-message")]
        public async Task<IActionResult> SendAudioMessage([FromBody] AudioMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Url))
            {
                return BadRequest("Número y URL del audio son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.AudioByUrl(dto.PhoneNumber, dto.Url)
                    : _whatsAppRequestBuilder.AudioReplyByUrl(dto.PhoneNumber, dto.Url, dto.ReplyToMessageId);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-document-message")]
        public async Task<IActionResult> SendDocumentMessage([FromBody] DocumentMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Url))
            {
                return BadRequest("Número y URL del documento son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.DocumentByUrl(dto.PhoneNumber, dto.Url, dto.Caption, dto.Filename)
                    : _whatsAppRequestBuilder.DocumentReplyByUrl(dto.PhoneNumber, dto.Url, dto.ReplyToMessageId, dto.Caption, dto.Filename);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-contact-message")]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactsMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.Contacts is null || !dto.Contacts.Any())
            {
                return BadRequest("Número y al menos un contacto son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.ContactsMessage(dto.PhoneNumber, dto.Contacts)
                    : _whatsAppRequestBuilder.ContactsReplyMessage(dto.PhoneNumber, dto.Contacts, dto.ReplyToMessageId);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-location-message")]
        public async Task<IActionResult> SendLocationMessage([FromBody] LocationMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.Latitude == 0 || dto.Longitude == 0)
            {
                return BadRequest("Número, latitud y longitud son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.LocationMessage(dto.PhoneNumber, dto.Latitude, dto.Longitude, dto.Name, dto.Address)
                    : _whatsAppRequestBuilder.LocationReplyMessage(dto.PhoneNumber, dto.Latitude, dto.Longitude, dto.ReplyToMessageId, dto.Name, dto.Address);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-list-message")]
        public async Task<IActionResult> SendListMessage([FromBody] ListMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.BodyText) || string.IsNullOrWhiteSpace(dto.ButtonText) || dto.Sections is null || !dto.Sections.Any())
            {
                return BadRequest("Número, texto del cuerpo, texto del botón y al menos una sección son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.ListMessage(dto.PhoneNumber, dto.HeaderText ?? string.Empty, dto.BodyText, dto.ButtonText, dto.Sections, dto.FooterText)
                    : _whatsAppRequestBuilder.ListReplyMessage(dto.PhoneNumber, dto.HeaderText ?? string.Empty, dto.BodyText, dto.ButtonText, dto.Sections, dto.ReplyToMessageId, dto.FooterText);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-reply-buttons-message")]
        public async Task<IActionResult> SendReplyButtonsMessage([FromBody] ReplyButtonsMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.BodyText) || dto.Buttons is null || !dto.Buttons.Any())
            {
                return BadRequest("Número, texto del cuerpo y al menos un botón son requeridos.");
            }

            try
            {
                object message = string.IsNullOrWhiteSpace(dto.ReplyToMessageId)
                    ? _whatsAppRequestBuilder.ReplyButtons(dto.PhoneNumber, dto.BodyText, dto.Buttons, dto.FooterText)
                    : _whatsAppRequestBuilder.ReplyButtonsReply(dto.PhoneNumber, dto.BodyText, dto.Buttons, dto.ReplyToMessageId, dto.FooterText);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-template")]
        public async Task<IActionResult> SendTemplateMessage([FromBody] TemplateMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.TemplateName))
            {
                return BadRequest(new MetaSendResult<WhatsAppResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = new WhatsAppErrorResponse
                    {
                        Error = new WhatsAppErrorResponse.ErrorBody { Message = "Número y nombre de la plantilla son requeridos." }
                    }
                });
            }

            try
            {
                object message = _whatsAppRequestBuilder.TypedTemplateMessage(dto);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MetaSendResult<WhatsAppResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Error = new WhatsAppErrorResponse
                    {
                        Error = new WhatsAppErrorResponse.ErrorBody 
                        { 
                            Message = "Error al enviar el mensaje.",
                            ErrorData = new WhatsAppErrorResponse.ErrorData
                            {
                                Details = ex.Message
                            }
                        }
                    }
                });
            }
        }

        [HttpPost("send-consent-code")]
        public async Task<IActionResult> SendConsentCode([FromBody] ConsentCodeMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return BadRequest("Número es requerido.");
            }

            try
            {
                object message = _whatsAppRequestBuilder.InteractiveCtaUrlConsentMessage(dto.PhoneNumber, dto.Code, dto.ExpiresInMinutes);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }

        [HttpPost("send-list-menu-aarco")]
        public async Task<IActionResult> SendListMenuAARCO([FromBody] TextMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return BadRequest("Número es requerido.");
            }

            try
            {
                object message = _whatsAppRequestBuilder.ListMenuAARCO(dto.PhoneNumber);

                var metaResponse = await _whatsAppMessageSender.ExecuteDetailedAsync(message);

                if (metaResponse.IsSuccess && metaResponse.Data is not null)
                {
                    return Ok(metaResponse.Data);
                }

                var status = (int)(metaResponse.StatusCode == default ? HttpStatusCode.BadGateway : metaResponse.StatusCode);
                return StatusCode(status, metaResponse.Error ?? new WhatsAppErrorResponse { Error = new WhatsAppErrorResponse.ErrorBody { Message = "Fallo al enviar el mensaje." } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar el mensaje.", detail = ex.Message });
            }
        }
    }
}