using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;
using static WAChatFlow.Shared.Contacts.ContactDto;
using static WAChatFlow.Shared.Messaging.WhatsApp.Requests.TemplateMapDto;

namespace WAChatFlow.Client.Pages.Templates
{
    public class TemplatePreviewBase : ComponentBase
    {
        [Inject] protected HttpClient Http { get; set; } = default!;

        [Parameter] public string TemplateId { get; set; } = default!;

        protected bool isLoading = true;
        protected string? loadError;

        protected TemplateDetailDto? template;
        protected List<UserStatusRow> users = new();
        protected long? SelectedUserId { get; set; }
        protected string PhoneNumber { get; set; } = string.Empty;

        protected bool hasHeaderImage = false;
        protected List<string> bodyParams = new();
        protected List<string> buttonQuickReplies = new();

        protected string? sendMessage;
        protected string? sendResult;
        protected string HeaderImageUrl { get; set; } = "";

        protected string AgentPhoneParam { get; set; } = "5215519618660";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var tResp = await Http.GetAsync($"api/templates/template-details/{Uri.EscapeDataString(TemplateId)}");

                if (!tResp.IsSuccessStatusCode)
                {
                    loadError = "No se pudo cargar la plantilla.";
                    return;
                }

                template = await tResp.Content.ReadFromJsonAsync<TemplateDetailDto>();

                if (template is null)
                {
                    loadError = "Plantilla no encontrada.";
                    return;
                }

                var uResp = await Http.GetAsync("api/users/catalog");

                if (uResp.IsSuccessStatusCode)
                {
                    users = await uResp.Content.ReadFromJsonAsync<List<UserStatusRow>>() ?? new();
                }

                InferParameters(template);
            }
            catch (Exception ex)
            {
                loadError = ex.Message;
            }
            finally
            {
                isLoading = false;
            }
        }

        protected void OnUserChanged(ChangeEventArgs e)
        {
            if (long.TryParse(Convert.ToString(e.Value), out var id))
            {
                SelectedUserId = id;
                var u = users.FirstOrDefault(x => x.UserId == id);
                if (u is not null && !string.IsNullOrWhiteSpace(u.PhoneNumber))
                    PhoneNumber = u.PhoneNumber;
            }
        }

        private void InferParameters(TemplateDetailDto t)
        {
            hasHeaderImage = false;
            bodyParams.Clear();
            buttonQuickReplies.Clear();

            if (t.Components is null) return;

            foreach (var comp in t.Components)
            {
                var type = (comp.Type ?? "").Trim().ToUpperInvariant();

                if (type == "HEADER" && string.Equals(comp.Format, "IMAGE", StringComparison.OrdinalIgnoreCase))
                    hasHeaderImage = true;

                if (type == "BODY")
                {
                    var placeholders = DetectPlaceholders(comp.Text);
                    var count = Math.Max(placeholders, 0);

                    if (count > 0 && comp.BodyExamples is not null && comp.BodyExamples.Count > 0)
                    {
                        foreach (var ex in comp.BodyExamples)
                        {
                            bodyParams.AddRange(ex);
                        }
                    }

                    while (bodyParams.Count < count)
                    {
                        bodyParams.Add(string.Empty);
                    }
                }

                if (type == "BUTTONS" && comp.Buttons is not null && comp.Buttons.Count > 0)
                {
                    foreach (var button in comp.Buttons)
                    {
                        var isUrl = string.Equals(button.Type, "URL", StringComparison.OrdinalIgnoreCase);
                        var hasVars = isUrl && !string.IsNullOrWhiteSpace(button.Url) && Regex.IsMatch(button.Url, @"\{\{\d+\}\}");
                        var isCopyCode = isUrl && !string.IsNullOrWhiteSpace(button.Url) &&
                                         button.Url.Contains("otp_type=COPY_CODE", StringComparison.OrdinalIgnoreCase);

                        if (!isUrl)
                        {
                            buttonQuickReplies.Add(string.IsNullOrWhiteSpace(button.Text) ? "" : button.Text);
                        }
                        else if (hasVars)
                        {
                            if (isCopyCode)
                                buttonQuickReplies.Add(bodyParams.Count > 0 ? bodyParams[0] : string.Empty);
                            else
                                buttonQuickReplies.Add(AgentPhoneParam); 
                        }
                        else
                        {
                            buttonQuickReplies.Add(string.Empty);
                        }
                    }
                }

            }
        }

        private static int DetectPlaceholders(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Regex.Matches(text, @"\{\{\d+\}\}").Count;
        }

        protected async Task SendTemplateAsync()
        {
            sendMessage = null;
            sendResult = null;

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                sendResult = "Debes indicar un número de WhatsApp.";
                return;
            }

            var components = new List<object>();

            if (template?.Components is not null)
            {
                var header = template.Components.FirstOrDefault(c =>
                    string.Equals(c.Type, "HEADER", StringComparison.OrdinalIgnoreCase));

                if (header is not null && string.Equals(header.Format, "IMAGE", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(HeaderImageUrl))
                    {
                        components.Add(new
                        {
                            type = "header",
                            parameters = new object[]
                            {
                                new { type = "image", image = new { link = HeaderImageUrl } }
                            }
                        });
                    }
                }

                var bodyComp = template.Components.FirstOrDefault(c =>
                    string.Equals(c.Type, "BODY", StringComparison.OrdinalIgnoreCase));

                if (bodyComp is not null)
                {
                    var placeholders = DetectPlaceholders(bodyComp.Text);
                    if (placeholders == 0)
                    {
                        components.Add(new { type = "body" });
                    }
                    else
                    {
                        var bodyParameters = new List<object>();
                        foreach (var param in bodyParams)
                        {
                            bodyParameters.Add(new { type = "text", text = param ?? string.Empty });
                        }

                        components.Add(new { type = "body", parameters = bodyParameters });
                    }
                }

                var btnComp = template.Components.FirstOrDefault(c =>
                    string.Equals(c.Type, "BUTTONS", StringComparison.OrdinalIgnoreCase));

                if (btnComp?.Buttons is not null && btnComp.Buttons.Count > 0)
                {
                    for (int i = 0; i < btnComp.Buttons.Count; i++)
                    {
                        var btn = btnComp.Buttons[i];
                        var btnType = (btn.Type ?? "").ToUpperInvariant();

                        if (btnType == "URL")
                        {
                            var hasVars = !string.IsNullOrWhiteSpace(btn.Url) && Regex.IsMatch(btn.Url, @"\{\{\d+\}\}");
                            if (!hasVars) continue;

                            var isCopyCode = btn.Url.Contains("otp_type=COPY_CODE", StringComparison.OrdinalIgnoreCase);

                            string rawValue;

                            if (isCopyCode)
                            {
                                rawValue = bodyParams.Count > 0 ? (bodyParams[0] ?? string.Empty) : string.Empty;

                                rawValue = Regex.Replace(rawValue.Trim(), "[^A-Za-z0-9]", "");

                                if (rawValue.Length > 15) rawValue = rawValue.Substring(0, 15);

                                if (string.IsNullOrEmpty(rawValue))
                                {
                                    sendResult = "El código de verificación no puede estar vacío para COPY_CODE.";
                                    return;
                                }
                            }
                            else
                            {
                                rawValue = (i < buttonQuickReplies.Count ? buttonQuickReplies[i] : "") ?? string.Empty;
                            }

                            var encoded = Uri.EscapeDataString(rawValue);

                            components.Add(new
                            {
                                type = "button",
                                sub_type = "url",
                                index = i.ToString(),
                                parameters = new object[]
                                {
                                    new { type = "text", text = encoded }
                                }
                            });
                        }
                    }
                }
            }

            var dto = new TemplateMessageDto
            {
                PhoneNumber = PhoneNumber.Trim(),
                TemplateName = template?.Name ?? "",
                LanguageCode = string.IsNullOrWhiteSpace(template?.Language) ? "es_MX" : template!.Language,
                Components = components
            };

            sendMessage = JsonSerializer.Serialize(new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = dto.PhoneNumber,
                type = "template",
                template = new
                {
                    name = dto.TemplateName,
                    language = new { code = dto.LanguageCode },
                    components
                }
            }, new JsonSerializerOptions { WriteIndented = true });

            var resp = await Http.PostAsJsonAsync("api/outbound-messages/send-template", dto);
            sendResult = resp.IsSuccessStatusCode
                ? "Mensaje enviado correctamente."
                : $"Error: {await resp.Content.ReadAsStringAsync()}";
        }
    }
}