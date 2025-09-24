using System.Text.Json;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;

namespace WAChatFlow.Server.Services.Messaging.Builders
{
    public sealed class WhatsAppRequestBuilder : IWhatsAppRequestBuilder
    {
        private const string messagingProduct = "whatsapp";

        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public object TextMessage(string to, string body, bool previewUrl = false)
        {
            return new
            {
                messaging_product = messagingProduct,
                recipient_type = "individual",
                to,
                type = "text",
                text = new { body, preview_url = previewUrl }
            };
        }

        public object TextReply(string to, string body, string replyToMessageId, bool previewUrl = false)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "text",
                context = new { message_id = replyToMessageId },
                text = new { body, preview_url = previewUrl }
            };
        }

        public object ReactionReply(string to, string reactToMessageId, string emoji)
        {
            return new
            {
                messaging_product = messagingProduct,
                recipient_type = "individual",
                to,
                type = "reaction",
                reaction = new { message_id = reactToMessageId, emoji = emoji ?? "" }
            };
        }

        public object ImageByUrl(string to, string url, string? caption = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "image",
                image = new { link = url, caption }
            };
        }

        public object ImageReplyByUrl(string to, string url, string replyToMessageId, string? caption = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "image",
                context = new { message_id = replyToMessageId },
                image = new { link = url, caption }
            };
        }

        public object AudioByUrl(string to, string url)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "audio",
                audio = new { link = url }
            };
        }

        public object AudioReplyByUrl(string to, string url, string replyToMessageId)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "audio",
                context = new { message_id = replyToMessageId },
                audio = new { link = url }
            };
        }

        public object DocumentByUrl(string to, string url, string? caption = null, string? filename = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "document",
                document = new { link = url, caption, filename }
            };
        }

        public object DocumentReplyByUrl(string to, string url, string replyToMessageId, string? caption = null, string? filename = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "document",
                context = new { message_id = replyToMessageId },
                document = new { link = url, caption, filename }
            };
        }

        public WhatsAppMessageRequest.Message ContactsMessage(string to, IEnumerable<ContactItemDto> contactItems)
        {
            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = to,
                Type = "contacts",
                Contacts = contactItems.Select(item => new WhatsAppMessageRequest.Contact
                {
                    Name = new WhatsAppMessageRequest.Name
                    {
                        FormattedName = item.FullName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        Prefix = item.Prefix,
                        Suffix = item.Suffix
                    },
                    Phones = item.Phones.Select(ph => new WhatsAppMessageRequest.Phone
                    {
                        PhoneNumber = ph.Phone,
                        WaId = ph.WaId,
                        Type = ph.Type
                    }).ToList()
                }).ToList()
            };
        }

        public WhatsAppMessageRequest.Message ContactsReplyMessage(string to, IEnumerable<ContactItemDto> contactItems, string replyToMessageId)
        {
            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = to,
                Type = "contacts",
                Context = new WhatsAppMessageRequest.Context {
                    MessageId = replyToMessageId
                },
                Contacts = contactItems.Select(item => new WhatsAppMessageRequest.Contact
                {
                    Name = new WhatsAppMessageRequest.Name
                    {
                        FormattedName = item.FullName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        Prefix = item.Prefix,
                        Suffix = item.Suffix
                    },
                    Phones = item.Phones.Select(ph => new WhatsAppMessageRequest.Phone
                    {
                        PhoneNumber = ph.Phone,
                        WaId = ph.WaId,
                        Type = ph.Type
                    }).ToList()
                }).ToList()
            };
        }

        public object LocationMessage(string to, double latitude, double longitude, string? name = null, string? address = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "location",
                location = new { latitude, longitude, name, address }
            };
        }

        public object LocationReplyMessage(string to, double latitude, double longitude, string replyToMessageId, string? name = null, string? address = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "location",
                context = new { message_id = replyToMessageId },
                location = new { latitude, longitude, name, address }
            };
        }

        public object ListMessage(string to, string? headerText, string bodyText, string buttonText, IEnumerable<ListSectionDto> sections, string? footerText = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "interactive",
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = headerText },
                    body = new { text = bodyText },
                    footer = footerText == null ? null : new { text = footerText },
                    action = new
                    {
                        button = buttonText,
                        sections = sections.Select(s => new
                        {
                            title = s.Title,
                            rows = s.Rows.Select(r => new
                            {
                                id = r.Id,
                                title = r.Title,
                                description = r.Description
                            }).ToArray()
                        }).ToArray()
                    }
                }
            };
        }

        public object ListReplyMessage(string to, string? headerText, string bodyText, string buttonText, IEnumerable<ListSectionDto> sections, string replyToMessageId, string? footerText = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "interactive",
                context = new { message_id = replyToMessageId },
                interactive = new
                {
                    type = "list",
                    header = new { type = "text", text = headerText },
                    body = new { text = bodyText },
                    footer = footerText == null ? null : new { text = footerText },
                    action = new
                    {
                        button = buttonText,
                        sections = sections.Select(s => new
                        {
                            title = s.Title,
                            rows = s.Rows.Select(r => new
                            {
                                id = r.Id,
                                title = r.Title,
                                description = r.Description
                            }).ToArray()
                        }).ToArray()
                    }
                }
            };
        }

        public object ReplyButtons(string to, string bodyText, IEnumerable<ReplyButtonDto> buttons, string? footerText = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new { text = bodyText },
                    footer = footerText == null ? null : new { text = footerText },
                    action = new
                    {
                        buttons = buttons.Select(b => new
                        {
                            type = "reply",
                            reply = new { id = b.Id, title = b.Title }
                        }).ToArray()
                    }
                }
            };
        }

        public object ReplyButtonsReply(string to, string bodyText, IEnumerable<ReplyButtonDto> buttons, string replyToMessageId, string? footerText = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "interactive",
                context = new { message_id = replyToMessageId },
                interactive = new
                {
                    type = "button",
                    body = new { text = bodyText },
                    footer = footerText == null ? null : new { text = footerText },
                    action = new
                    {
                        buttons = buttons.Select(b => new
                        {
                            type = "reply",
                            reply = new { id = b.Id, title = b.Title }
                        }).ToArray()
                    }
                }
            };
        }

        public object TemplateMessage(string to, string templateName, string languageCode = "es_MX", object? components = null)
        {
            return new
            {
                messaging_product = messagingProduct,
                to,
                type = "template",
                template = new
                {
                    name = templateName,
                    language = new { code = languageCode },
                    components
                }
            };
        }

        public object MarkAsRead(string messageId)
        {
            return new
            {
                messaging_product = messagingProduct,
                status = "read",
                message_id = messageId
            };
        }

        public WhatsAppMessageRequest.Message InteractiveButtonsMessage(ReplyButtonsMessageDto dto)
        {
            if (dto.BodyText.Length > 1024)
            {
                dto.BodyText = dto.BodyText.Substring(0, 1024);
            }

            if (dto.FooterText?.Length > 60)
            {
                dto.FooterText = dto.FooterText.Substring(0, 60);
            }

            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = dto.PhoneNumber,
                Type = "interactive",
                Interactive = new WhatsAppMessageRequest.Interactive
                {
                    Type = "button",
                    Body = new WhatsAppMessageRequest.InteractiveBody { Text = dto.BodyText },
                    Footer = new WhatsAppMessageRequest.InteractiveFooter { Text = dto.FooterText },
                    Action = new WhatsAppMessageRequest.InteractiveAction
                    {
                        Buttons = dto.Buttons.Select(b => new WhatsAppMessageRequest.Button
                        {
                            Reply = new WhatsAppMessageRequest.Reply { Id = b.Id, Title = b.Title }
                        }).ToList()
                    }
                }
            };
        }

        public WhatsAppMessageRequest.Message InteractiveButtonsConsentCode(ReplyButtonsMessageDto dto, string code, string validationUrl, int expiresInMinutes = 5)
        {
            var greeting = string.IsNullOrWhiteSpace(dto.BodyText) ? "Estimado cliente," : dto.BodyText.Trim();

            var body =
                $"{greeting}\n\n" +
                $"Tu código de validación es: {code} 🔑\n\n" +
                $"Valídalo aquí: {validationUrl}\n\n" +
                $"Por seguridad, no compartas este código.";

            var footer = string.IsNullOrWhiteSpace(dto.FooterText)
                ? $"Este código expira en {expiresInMinutes} minutos."
                : dto.FooterText.Trim();

            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = "whatsapp",
                RecipientType = "individual",
                To = dto.PhoneNumber,
                Type = "interactive",
                Interactive = new WhatsAppMessageRequest.Interactive
                {
                    Type = "button",
                    Body = new WhatsAppMessageRequest.InteractiveBody { Text = body },
                    Footer = new WhatsAppMessageRequest.InteractiveFooter { Text = footer },
                    Action = new WhatsAppMessageRequest.InteractiveAction
                    {
                        Buttons = dto.Buttons
                            .Take(3)
                            .Select(b => new WhatsAppMessageRequest.Button
                            {
                                Reply = new WhatsAppMessageRequest.Reply
                                {
                                    Id = b.Id, Title = b.Title
                                }
                            })
                            .ToList()
                    }
                }
            };
        }

        public WhatsAppMessageRequest.Message InteractiveCtaUrlConsentMessage(string to, string code, int expiresInMinutes = 5)
        {
            var body =
                $"Estimado cliente,\n\n" +
                $"Tu código de validación es: {code} 🔑\n\n" +
                $"Por seguridad, no compartas este código.\n\n";

            var footer = $"Este código expira en {expiresInMinutes} minutos.";

            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = to,
                Type = "interactive",
                Interactive = new WhatsAppMessageRequest.Interactive
                {
                    Type = "cta_url",
                    Body = new WhatsAppMessageRequest.InteractiveBody { Text = body },
                    Footer = new WhatsAppMessageRequest.InteractiveFooter { Text = footer },
                    Action = new WhatsAppMessageRequest.InteractiveAction
                    {
                        Name = "cta_url",
                        Parameters = new WhatsAppMessageRequest.CtaParameters
                        {
                            DisplayText = "Dar Consentimiento",
                            Url = "https://sia.aarco.com.mx"
                        }
                    }
                }
            };
        }

        public WhatsAppMessageRequest.Message ListMenuAARCO(string to)
        {
            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = to,
                Type = "interactive",
                Interactive = new WhatsAppMessageRequest.Interactive
                {
                    Type = "list",
                    Header = new WhatsAppMessageRequest.InteractiveHeader { Type = "text", Text = "¿Como puedo ayudarte hoy?" },
                    Body = new WhatsAppMessageRequest.InteractiveBody { Text = "Selecciona una opción del menú:" },
                    Footer = new WhatsAppMessageRequest.InteractiveFooter { Text = "Atención AARCO" },
                    Action = new WhatsAppMessageRequest.InteractiveAction
                    {
                        Button = "Opciones",
                        Sections = new List<WhatsAppMessageRequest.Section>
                        {
                            new WhatsAppMessageRequest.Section
                            {
                                Title = "Consultas",
                                Rows = new List<WhatsAppMessageRequest.Row>
                                {
                                    new WhatsAppMessageRequest.Row
                                    {
                                        Id = "consent_status",
                                        Title = "Estado de Consentimiento"
                                    },
                                    new WhatsAppMessageRequest.Row
                                    {
                                        Id = "contact_agent",
                                        Title = "Contactar a mi Agente"
                                    }
                                }
                            },
                            new WhatsAppMessageRequest.Section
                            {
                                Title = "Documentos",
                                Rows = new List<WhatsAppMessageRequest.Row>
                                {
                                    new WhatsAppMessageRequest.Row
                                    {
                                        Id = "send_document",
                                        Title = "Recibir mi Poliza"
                                    },
                                    new WhatsAppMessageRequest.Row
                                    {
                                        Id = "payment_receipt",
                                        Title = "Recibo de Pago"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public WhatsAppMessageRequest.Message TypedTextMessage(TextMessageDto dto)
        {
            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = dto.PhoneNumber,
                Type = "text",
                Text = new WhatsAppMessageRequest.Text
                {
                    Body = dto.Body,
                    PreviewUrl = dto.PreviewUrl
                }
            };
        }

        public WhatsAppMessageRequest.Message TypedTextMessageReply(TextMessageDto dto)
        {
            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = messagingProduct,
                RecipientType = "individual",
                To = dto.PhoneNumber,
                Type = "text",
                Context = new WhatsAppMessageRequest.Context
                {
                    MessageId = dto.ReplyToMessageId
                },
                Text = new WhatsAppMessageRequest.Text
                {
                    Body = dto.Body,
                    PreviewUrl = dto.PreviewUrl
                }
            };
        }

        public WhatsAppMessageRequest.Message TypedTemplateMessage(TemplateMessageDto dto)
        {
            var toTemplateComponents = ConvertAnonymousToTemplateComponents(dto.Components);

            return new WhatsAppMessageRequest.Message
            {
                MessagingProduct = "whatsapp",
                RecipientType = "individual",
                To = dto.PhoneNumber,
                Type = "template",
                Template = new WhatsAppMessageRequest.Template
                {
                    Name = dto.TemplateName,
                    Language = new WhatsAppMessageRequest.TemplateLanguage
                    {
                        Code = string.IsNullOrWhiteSpace(dto.LanguageCode) ? "es_MX" : dto.LanguageCode
                    },
                    Components = toTemplateComponents ?? new List<WhatsAppMessageRequest.TemplateComponent>()
                }
            };
        }

        private static List<WhatsAppMessageRequest.TemplateComponent> ConvertAnonymousToTemplateComponents(IEnumerable<object>? components)
        {
            var listComponents = new List<WhatsAppMessageRequest.TemplateComponent>();

            if (components != null)
            {
                foreach (var item in components)
                {
                    var json = JsonSerializer.Serialize(item, jsonOptions);
                    var comp = JsonSerializer.Deserialize<WhatsAppMessageRequest.TemplateComponent>(json, jsonOptions);
                    if (comp != null) listComponents.Add(comp);
                }
            }

            return listComponents;
        }
    }
}