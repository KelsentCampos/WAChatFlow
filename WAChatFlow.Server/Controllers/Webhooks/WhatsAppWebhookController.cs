using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using WAChatFlow.Server.Configuration.Options;
using WAChatFlow.Server.Infrastructure.Repositories.Consent;
using WAChatFlow.Server.Infrastructure.Repositories.Contacts;
using WAChatFlow.Server.Services.Messaging.Builders;
using WAChatFlow.Server.Services.Messaging.Senders;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Messaging.Webhook;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;

namespace WAChatFlow.Server.Controllers.Webhooks
{
    [ApiController]
    [Route("api/whatsapp-aarco")]
    public class WhatsAppWebhookController : ControllerBase
    {
        private readonly IWhatsAppMessageSender _whatsAppCloudSend;
        private readonly IWhatsAppRequestBuilder _whatsAppRequestBuilder;
        private readonly IContactsRepository _contactsRepository;
        private readonly IConsentRepository _consentRepository;

        private const string channel = Constants.Channels.WhatsApp;

        private readonly string _verifyToken;

        public WhatsAppWebhookController(IWhatsAppMessageSender whatsAppCloudSend, IWhatsAppRequestBuilder whatsAppRequestBuilder,
            IContactsRepository contactsRepository, IConsentRepository consentRepository ,IOptions<WebhookVerificationOptions> options)
        {
            _whatsAppCloudSend = whatsAppCloudSend;
            _whatsAppRequestBuilder = whatsAppRequestBuilder;
            _contactsRepository = contactsRepository;
            _consentRepository = consentRepository;
            _verifyToken = options.Value.VerifyToken;
        }

        [HttpGet]
        public IActionResult VerifyToken()
        {
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (string.IsNullOrEmpty(token) || token != _verifyToken)
            {
                return Unauthorized("Token no válido");
            }

            return Ok(challenge);
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] WhatsAppWebhookEvent webhook)
        {
            if (webhook.Object != "whatsapp_business_account")
            {
                return BadRequest("Objeto no válido");
            }

            object? responseMessage = null;

            foreach (var entry in webhook.Entry)
            {
                foreach (var change in entry.Changes)
                {
                    var message = change.Value.Messages?.FirstOrDefault();

                    if (message is not null)
                    {
                        var from = message.From;
                        var text = message?.Text?.Body;

                        var dbUser = await _contactsRepository.GetByPhoneAsync(from);

                        switch (message?.Type)
                        {
                            case "text":
                                responseMessage = new
                                {
                                    messaging_product = channel,
                                    to = from,
                                    type = "text",
                                    text = new
                                    {
                                        body = $"Escribiste: {text}"
                                    }
                                };
                                break;
                            case "button":
                                var buttonPayload = message.Button?.Payload;

                                switch (buttonPayload)
                                {
                                    case "confirmar-consent":

                                        var code = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 999999).ToString("");

                                        await _consentRepository.CreateAsync(dbUser.UserId, channel, code, TimeSpan.FromMinutes(5));

                                        TemplateMessageDto templateCode = new TemplateMessageDto
                                        {
                                            PhoneNumber = from,
                                            TemplateName = "template_consent_otp",
                                            LanguageCode = "es_MX",
                                            Components = new object[]
                                            {
                                                new
                                                {
                                                    type = "body",
                                                    parameters = new object[]
                                                    {
                                                        new { type = "text", text = code.ToString() } 
                                                    }
                                                },
                                                
                                                new
                                                {
                                                    type = "button",
                                                    sub_type = "url",
                                                    index = "0",
                                                    parameters = new object[]
                                                    {
                                                        new { type = "text", text = code.ToString() }
                                                    }
                                                }
                                            }
                                        };

                                        object messageCode = _whatsAppRequestBuilder.TypedTemplateMessage(templateCode);

                                        var metaResponse = await _whatsAppCloudSend.ExecuteDetailedAsync(messageCode);

                                        TemplateMessageDto templateLinkVerify = new TemplateMessageDto
                                        {
                                            PhoneNumber = from,
                                            TemplateName = "plantilla_verificacion_cliente",
                                            LanguageCode = "es",
                                            Components = new object[]
                                            {
                                                new
                                                {
                                                    type = "body",
                                                    parameters = new object[]
                                                    {
                                                        new { type = "text", text = dbUser.FullName } 
                                                    }
                                                }
                                            }
                                        };
                                        
                                        object messageLink = _whatsAppRequestBuilder.TypedTemplateMessage(templateLinkVerify);

                                        var response = await _whatsAppCloudSend.ExecuteDetailedAsync(messageLink);

                                        break;

                                    case "denegar-consent":
                                        
                                        string messageDenegar = "Entendido, no volverás a recibir notificaciones de AARCO.";

                                        responseMessage = _whatsAppRequestBuilder.TextMessage(from, messageDenegar, false);

                                        break;
                                }

                                break;
                        }
                    }

                    await _whatsAppCloudSend.ExecuteWithResultAsync(responseMessage ?? new object());
                }
            }

            return Ok("EVENT_RECEIVED");
        }
    }
}
