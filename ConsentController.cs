using Microsoft.AspNetCore.Mvc;
using WAChatFlow.Server.Infrastructure.Repositories.Consent;
using WAChatFlow.Server.Infrastructure.Repositories.Contacts;
using WAChatFlow.Server.Services.Messaging.Builders;
using WAChatFlow.Server.Services.Messaging.Senders;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Consent;

namespace WAChatFlow.Server.Controllers.Consent
{
    [ApiController]
    [Route("api/consent")]
    public class ConsentController : ControllerBase
    {
        private readonly IWhatsAppMessageSender _whatsAppCloudSend;
        private readonly IWhatsAppRequestBuilder _whatsAppRequestBuilder;
        private readonly IContactsRepository _contactsRepository;
        private readonly IConsentRepository _consentRepository;

        private const string channel = Constants.Channels.WhatsApp;

        public ConsentController(IWhatsAppMessageSender whatsAppMessageSender, IWhatsAppRequestBuilder whatsAppRequestBuilder, 
            IContactsRepository contactsRepository, IConsentRepository consentRepository)
        {
            _whatsAppCloudSend = whatsAppMessageSender;
            _whatsAppRequestBuilder = whatsAppRequestBuilder;
            _contactsRepository = contactsRepository;
            _consentRepository = consentRepository;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("Consent service is running.");
        }

        [HttpPost("verify-consent-code")]
        public async Task<IActionResult> VerifyConsentCode([FromBody] VerifyConsentDto dto)
        {
            if (dto.UserId <= 0 && string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest("El usuario y el codigo son obligatorios");
            }

            try
            {            
                var codeOk = await _consentRepository.VerifyAndConsumeAsync(dto.UserId, channel, dto.Code);

                if (!codeOk)
                {
                    return BadRequest("Código inválido o expirado.");
                }

                var userInfo = await _contactsRepository.GetByIdAsync(dto.UserId);

                await _consentRepository.MarkConsentValidatedAsync(dto.UserId, channel, dto.Wamid);

                string responseMessageUser = "¡Código verificado! Tu consentimiento quedó activo ✅";

                object message = _whatsAppRequestBuilder.TextMessage(userInfo.PhoneNumber, responseMessageUser, false);

                await _whatsAppCloudSend.ExecuteDetailedAsync(message);
                
                return Ok(new { Message = "Consentimiento verificado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
