using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Common.Utilities;
using WAChatFlow.Shared.Contacts;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;
using WAChatFlow.Shared.Messaging.WhatsApp.Responses;

namespace WAChatFlow.Client.Pages.Contacts
{
    public class ContactsBase : ComponentBase
    {
        protected readonly HashSet<long> _editing = new();
        protected readonly Dictionary<long, string> _editBuffer = new();

        [Inject] protected HttpClient Http { get; set; }

        protected List<ContactDto.UserStatusRow> users = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var data = await Http.GetFromJsonAsync<IEnumerable<ContactDto.UserStatusRow>>("api/users/catalog"); users = data?.ToList() ?? new List<ContactDto.UserStatusRow>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar catálogo: {ex.Message}");
            }
        }

        protected string GetBadgeClass(Enums.ConsentStatus consentStatus)
        {
            switch (consentStatus)
            {
                case Enums.ConsentStatus.VALIDATED:
                    return "bg-success";

                case Enums.ConsentStatus.PENDING:
                    return "bg-warning";

                case Enums.ConsentStatus.REJECTED:
                    return "bg-danger";

                case Enums.ConsentStatus.COOLDOWN:
                    return "bg-info";

                case Enums.ConsentStatus.REVOKED:
                    return "bg-dark";

                default:
                    return "bg-secondary";
            }
        }

        protected string GetConsentText(Enums.ConsentStatus consentStatus)
        {
            switch (consentStatus)
            {
                case Enums.ConsentStatus.VALIDATED:
                    return "Consentimiento Válido";

                case Enums.ConsentStatus.PENDING:
                    return "Pendiente de Validar";

                case Enums.ConsentStatus.REJECTED:
                    return "Rechazado por el Usuario";

                case Enums.ConsentStatus.COOLDOWN:
                    return "En Espera de Reintento";

                case Enums.ConsentStatus.REVOKED:
                    return "Consentimiento Revocado";

                default:
                    return "Desconocido";
            }
        }

        protected RenderFragment GetActionButton(ContactDto.UserStatusRow user)
        {
            return builder =>
            {
                switch (user.ConsentStatus)
                {
                    case Enums.ConsentStatus.VALIDATED:
                        builder.OpenElement(0, "button");
                        builder.AddAttribute(1, "class", "btn btn-outline btn-sm w-auto");
                        builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => Reenviar(user)));
                        builder.AddContent(3, "Reenviar");
                        builder.CloseElement();
                        break;

                    case Enums.ConsentStatus.PENDING:
                        builder.OpenElement(0, "button");
                        builder.AddAttribute(1, "class", "btn btn-primary btn-sm w-auto");
                        builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => EnviarConsentimiento(user)));
                        builder.AddContent(3, "Enviar Consentimiento");
                        builder.CloseElement();
                        break;

                    case Enums.ConsentStatus.REJECTED:
                        builder.OpenElement(0, "button");
                        builder.AddAttribute(1, "class", "btn btn-primary btn-sm w-auto");
                        builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => Reintentar(user)));
                        builder.AddContent(3, "Reintentar");
                        builder.CloseElement();
                        break;

                    case Enums.ConsentStatus.COOLDOWN:
                        builder.OpenElement(0, "span");
                        builder.AddContent(1, "En espera");
                        builder.CloseElement();
                        break;

                    case Enums.ConsentStatus.REVOKED:
                        builder.OpenElement(0, "span");
                        builder.AddContent(1, "Revocado");
                        builder.CloseElement();
                        break;

                    default:
                        builder.OpenElement(0, "span");
                        builder.AddContent(1, "Sin acción");
                        builder.CloseElement();
                        break;
                }
            };
        }

        protected string FormatPhone(string phoneNumber)
        {
            return PhoneNumberUtil.FormatPhoneForDisplay(phoneNumber);
        }

        protected async Task EnviarConsentimiento(ContactDto.UserStatusRow user)
        {
            try
            {
                var recipientWaId = PhoneNumberUtil.NormalizeToWhatsAppId(user.PhoneNumber);

                var fullName = string.IsNullOrWhiteSpace(user.FullName) ? "Cliente" : user.FullName.Trim();

                var parameters = new List<WhatsAppMessageRequest.TemplateParameter>
                {
                    new() { Type = "text", Text = fullName }
                };

                var components = new List<WhatsAppMessageRequest.TemplateComponent>
                {
                    new WhatsAppMessageRequest.TemplateComponent
                    {
                        Type = "body",
                        Parameters = parameters
                    },
                    
                    new WhatsAppMessageRequest.TemplateComponent
                    {
                        Type = "button",
                        SubType = "quick_reply",
                        Index = "0",
                        Parameters = new List<WhatsAppMessageRequest.TemplateParameter>
                        {
                            new WhatsAppMessageRequest.TemplateParameter
                            {
                                Type = "payload",
                                Payload = "confirmar-consent"
                            }
                        }
                    },

                    new WhatsAppMessageRequest.TemplateComponent
                    {
                        Type = "button",
                        SubType = "quick_reply",
                        Index = "1",
                        Parameters = new List<WhatsAppMessageRequest.TemplateParameter>
                        {
                            new WhatsAppMessageRequest.TemplateParameter
                            {
                                Type = "payload",
                                Payload = "denegar-consent"
                            }
                        }
                    }
                };

                var dto = new TemplateMessageDto
                {
                    PhoneNumber = recipientWaId,
                    TemplateName = "template_welcome_message_aarco",
                    LanguageCode = "es_MX",
                    Components = components
                };

                var httpResp = await Http.PostAsJsonAsync("api/outbound-messages/send-template", dto);

                if (httpResp.IsSuccessStatusCode)
                {
                    var result = await httpResp.Content.ReadFromJsonAsync<MetaSendResult<WhatsAppResponse>>();

                    if (result is not null && (result.Data is not null || result.Error is not null))
                    {
                        if (result.IsSuccess && result.Data is not null)
                        {
                            var wamid = result.Data.Messages?.FirstOrDefault()?.Id;
                            var waId = result.Data.Contacts?.FirstOrDefault()?.WaId;
                            Console.WriteLine($"✅ OK. WAMID: {wamid}  WaId:{waId}");
                        }
                        else
                        {
                            var error = result.Error?.Error;
                            Console.WriteLine($"❌ Meta error {result.StatusCode} - {error?.Message} (code:{error?.Code} sub:{error?.ErrorSubcode} trace:{error?.FbTraceId} details:{error?.ErrorData?.Details})");
                        }
                        return;
                    }

                    var waResponse = await httpResp.Content.ReadFromJsonAsync<WhatsAppResponse>();

                    if (waResponse is not null)
                    {
                        var wamid = waResponse.Messages?.FirstOrDefault()?.Id;
                        var waId = waResponse.Contacts?.FirstOrDefault()?.WaId;
                        Console.WriteLine($"✅ OK. WAMID: {wamid}  WaId:{waId}");
                        return;
                    }

                    Console.WriteLine("⚠️ Éxito HTTP, pero el cuerpo vino vacío.");
                    return;
                }

                var metaErr = await httpResp.Content.ReadFromJsonAsync<WhatsAppErrorResponse>();

                if (metaErr?.Error is not null)
                {
                    var error = metaErr.Error;
                    Console.WriteLine($"❌ HTTP {(int)httpResp.StatusCode} {httpResp.StatusCode} | msg:{error.Message} type:{error.Type} code:{error.Code} sub:{error.ErrorSubcode} details:{error.ErrorData?.Details} trace:{error.FbTraceId}");
                }
                else
                {
                    var raw = await httpResp.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ HTTP {(int)httpResp.StatusCode} {httpResp.StatusCode} | body: {raw}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al enviar consentimiento: {ex.Message}");
            }
        }

        protected async Task Reenviar(ContactDto.UserStatusRow user)
        {
            Console.WriteLine($"🔄 Reenviando a {user.FullName}");
            await EnviarConsentimiento(user);
        }

        protected async Task Reintentar(ContactDto.UserStatusRow user)
        {
            Console.WriteLine($"⏳ Reintentando con {user.FullName}");
            await EnviarConsentimiento(user);
        }

        protected async Task DisableEditAsync(ContactDto.UserStatusRow u)
        {
            string newName;

            if (_editBuffer.TryGetValue(u.UserId, out newName))
            {
                if (string.IsNullOrWhiteSpace(newName))
                {
                    newName = "";
                }
                else
                {
                    newName = newName.Trim();
                }

                int idx = users.FindIndex(x => x.UserId == u.UserId);
                if (idx >= 0)
                {
                    users[idx] = users[idx] with { FullName = newName };
                }

                await SaveNameAsync(u.PhoneNumber, newName);
            }

            if (_editing.Contains(u.UserId))
            {
                _editing.Remove(u.UserId);
            }

            if (_editBuffer.ContainsKey(u.UserId))
            {
                _editBuffer.Remove(u.UserId);
            }

            StateHasChanged();
        }

        protected async Task SaveNameAsync(string phoneNumber, string fullName)
        {
            var payload = new { phoneNumber = phoneNumber, fullName = fullName };

            var resp = await Http.PostAsJsonAsync("api/users/upsert-name", payload);

            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                Console.WriteLine("❌ No se pudo guardar: "
                                  + ((int)resp.StatusCode).ToString()
                                  + " " + resp.StatusCode.ToString()
                                  + " | " + err);
            }
            else
            {
                Console.WriteLine("✅ Nombre guardado.");
            }
        }

        protected bool IsEditing(ContactDto.UserStatusRow u)
        {
            if (_editing.Contains(u.UserId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void EnableEdit(ContactDto.UserStatusRow u)
        {
            if (!_editing.Contains(u.UserId))
            {
                _editing.Add(u.UserId);
            }

            if (_editBuffer.ContainsKey(u.UserId))
            {
                _editBuffer[u.UserId] = u.FullName;
            }
            else
            {
                _editBuffer.Add(u.UserId, u.FullName);
            }

            StateHasChanged();
        }

        protected string GetBuffer(ContactDto.UserStatusRow u)
        {
            if (_editBuffer.TryGetValue(u.UserId, out var v))
            {
                return v;
            }
            else
            {
                return u.FullName;
            }
        }

        protected void OnNameChanged(ContactDto.UserStatusRow u, string value)
        {
            if (_editBuffer.ContainsKey(u.UserId))
            {
                _editBuffer[u.UserId] = value;
            }
            else
            {
                _editBuffer.Add(u.UserId, value);
            }
        }

        protected async Task OnNameKeyDown(KeyboardEventArgs e, ContactDto.UserStatusRow u)
        {
            if (e.Key == "Enter")
            {
                await DisableEditAsync(u);   // guarda
            }
            else if (e.Key == "Escape")
            {
                CancelEdit(u);              // cancela
            }
        }

        protected void CancelEdit(ContactDto.UserStatusRow u)
        {
            if (_editing.Contains(u.UserId))
            {
                _editing.Remove(u.UserId);
            }

            if (_editBuffer.ContainsKey(u.UserId))
            {
                _editBuffer.Remove(u.UserId);
            }

            StateHasChanged();
        }
    }
}
