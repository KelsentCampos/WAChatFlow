using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WAChatFlow.Server.Configuration.Webhook;
using WAChatFlow.Shared.Interfaces.WhatsApp;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Models.WhatsApp.Requests;
using WAChatFlow.Shared.Models.WhatsApp.Webhook;

namespace WAChatFlow.Server.Controllers
{
    [ApiController]
    [Route("api/whatsapp-aarco")]
    public class WhatsAppWebhookController : ControllerBase
    {
        private readonly IWhatsAppMessageSender _whatsAppCloudSend;
        private readonly WhatsAppWebhookEvent _whatsAppWebhookModel;
        private readonly WhatsAppMessageRequest _whatsAppCloudModel;

        private const string channel = Constants.Channels.WhatsApp;

        private readonly string _verifyToken;

        public WhatsAppWebhookController(IWhatsAppMessageSender whatsAppCloudSend, WhatsAppWebhookEvent whatsAppWebhookModel, WhatsAppMessageRequest whatsAppCloudModel, IOptions<WebhookVerificationOptions> options)
        {
            _whatsAppCloudSend = whatsAppCloudSend;
            _whatsAppCloudModel = whatsAppCloudModel;
            _whatsAppWebhookModel = whatsAppWebhookModel;
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
            foreach (var entry in webhook.Entry)
            {
                foreach (var change in entry.Changes)
                {
                    var message = change.Value.Messages?.FirstOrDefault();
                    if (message != null && message.Type == "text")
                    {
                        var from = message.From;
                        var text = message.Text.Body;

                        var responseMessage = new
                        {
                            messaging_product = channel,
                            to = from,
                            type = "text",
                            text = new
                            {
                                body = $"Echo: {text}"
                            }
                        };

                        await _whatsAppCloudSend.ExecuteAsync(responseMessage);
                    }
                }
            }

            return Ok("EVENT_RECEIVED");
        }
    }
}
