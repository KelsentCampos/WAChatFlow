using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Common.Utilities;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;
using WAChatFlow.Shared.Messaging.WhatsApp.Responses;

namespace WAChatFlow.Client.Pages.Messages
{
    public class MessagesBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; } = null!;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected bool ShowErrorModal { get; set; } = false;

        protected TextMessageDto MessageDto = new TextMessageDto();

        private static readonly JsonSerializerOptions jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public string _messageType = "text";
        public string PhoneNumber = string.Empty;
        public string MessageText = string.Empty;
        public string PreviewText = string.Empty;

        protected override Task OnInitializedAsync()
        {
            MessageDto = new TextMessageDto
            {
                PhoneNumber = string.Empty,
                Body = string.Empty,
                PreviewUrl = false,
                ReplyToMessageId = null
            };

            return Task.CompletedTask;
        }

        protected void ToggleMessageOptions(ChangeEventArgs e)
        {
            var value = e.Value == null ? string.Empty : e.Value.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                _messageType = "text";
            }
            else
            {
                _messageType = value;
            }
        }

        protected async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(MessageText))
            {
                ErrorMessage = "Completa el número y el mensaje.";
                ShowErrorModal = true;
                return;
            }

            var recipientWaId = PhoneNumberUtil.NormalizeToWhatsAppId(PhoneNumber);

            MessageDto.PhoneNumber = recipientWaId;
            MessageDto.Body = MessageText;
            MessageDto.PreviewUrl = false;
            MessageDto.ReplyToMessageId = null;

            string endpoint = GetEndpointForCurrentType();

            if (endpoint.Length == 0)
            {
                ErrorMessage = "Por ahora solo se envían mensajes de texto desde esta página.";
                ShowErrorModal = true;
                return;
            }

            try
            {
                var httpResponse = await Http.PostAsJsonAsync(endpoint, MessageDto);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var waResponse = await httpResponse.Content.ReadFromJsonAsync<MetaSendResult<WhatsAppResponse>>();

                    if (waResponse != null)
                    {
                        string wamid = waResponse.Data?.Messages?.FirstOrDefault()?.Id;
                        string waId = waResponse.Data?.Contacts?.FirstOrDefault()?.WaId;

                        ShowErrorModal = false;
                        PreviewText = string.Empty;
                        Console.WriteLine("✅ OK. Enviado a " + waId + " WAMID: " + wamid);
                        return;
                    }

                    ErrorMessage = "⚠️ Éxito HTTP, pero el cuerpo vino vacío.";
                    ShowErrorModal = true;
                    return;
                }
                var raw = await httpResponse.Content.ReadAsStringAsync();

                MetaSendResult<WhatsAppResponse> wrapped = null;
                WhatsAppErrorResponse? metaError = null;

                try
                {
                    wrapped = JsonSerializer.Deserialize<MetaSendResult<WhatsAppResponse>>(raw, jsonOptions);
                }
                catch (Exception ex)
                {
                    ErrorMessage = @"⚠️ No se pudo parsear el error JSON: " + ex.Message;
                }

                if (wrapped is not null)
                {
                    var e = wrapped.Error?.Error;

                    ErrorMessage =
                        e?.ErrorUserMsg
                        ?? e?.Message
                        ?? wrapped.Error?.Error.ErrorData?.Details
                        ?? $"HTTP {(int)httpResponse.StatusCode}";
                    ShowErrorModal = true;
                    StateHasChanged();
                    return;
                }

                try 
                { 
                    metaError = JsonSerializer.Deserialize<WhatsAppErrorResponse>(raw, jsonOptions);

                    if (metaError != null && metaError.Error != null)
                    {
                        var err = metaError.Error.ErrorData;
                        ErrorMessage = @"
                          ❌ HTTP " + (int)httpResponse.StatusCode + " " + httpResponse.StatusCode
                            + " | msg:" + err.Details;
                        ShowErrorModal = true;
                        StateHasChanged();
                        return;
                    }
                } 
                catch 
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Excepción al enviar el mensaje: " + ex.Message);
            }
        }

        protected void PreviewMessage()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(MessageText))
            {
                ErrorMessage = "Completa el número y el mensaje.";
                ShowErrorModal = true;
                return;
            }

            PreviewText = "<strong>[" + _messageType.ToUpper() + "]</strong> Para: " + PhoneNumber + "<br>" + MessageText;
            Console.WriteLine("Mensaje \"" + _messageType + "\" listo para " + PhoneNumber);
        }

        private string GetEndpointForCurrentType()
        {
            if (_messageType == "text")
            {
                return "api/outbound-messages/send-text-message";
            }

            return string.Empty;
        }
    }
}
