using WAChatFlow.Shared.Messaging.WhatsApp.Requests;

namespace WAChatFlow.Server.Services.Messaging.Builders
{
    public interface IWhatsAppRequestBuilder
    {
        object TextMessage(string to, string body, bool previewUrl = false);
        object TextReply(string to, string body, string replyToMessageId, bool previewUrl = false);

        object ReactionReply(string to, string reactToMessageId, string emoji);

        object ImageByUrl(string to, string url, string? caption = null);
        object ImageReplyByUrl(string to, string url, string replyToMessageId, string? caption = null);

        object AudioByUrl(string to, string url);
        object AudioReplyByUrl(string to, string url, string replyToMessageId);

        object DocumentByUrl(string to, string url, string? caption = null, string? filename = null);
        object DocumentReplyByUrl(string to, string url, string replyToMessageId, string? caption = null, string? filename = null);

        WhatsAppMessageRequest.Message ContactsMessage(string to, IEnumerable<ContactItemDto> contactItems);
        WhatsAppMessageRequest.Message ContactsReplyMessage(string to, IEnumerable<ContactItemDto> contacts, string replyToMessageId);

        object LocationMessage(string to, double latitude, double longitude, string? name = null, string? address = null);
        object LocationReplyMessage(string to, double latitude, double longitude, string replyToMessageId, string? name = null, string? address = null);

        object ListMessage(string to, string headerText, string bodyText, string buttonText, IEnumerable<ListSectionDto> sections, string? footerText = null);
        object ListReplyMessage(string to, string headerText, string bodyText, string buttonText, IEnumerable<ListSectionDto> sections, string replyToMessageId, string? footerText = null);

        object ReplyButtons(string to, string bodyText, IEnumerable<ReplyButtonDto> buttons, string? footerText = null);
        object ReplyButtonsReply(string to, string bodyText, IEnumerable<ReplyButtonDto> buttons, string replyToMessageId, string? footerText = null);

        object TemplateMessage(string to, string templateName, string languageCode = "es_MX", object? components = null);

        object MarkAsRead(string messageId);

        WhatsAppMessageRequest.Message InteractiveButtonsMessage(ReplyButtonsMessageDto dto);
        WhatsAppMessageRequest.Message InteractiveButtonsConsentCode(ReplyButtonsMessageDto dto, string code, string validationUrl, int expiresInMinutes = 5);
        WhatsAppMessageRequest.Message InteractiveCtaUrlConsentMessage(string to, string code, int expiresInMinutes = 5);

        WhatsAppMessageRequest.Message ListMenuAARCO(string to);

        WhatsAppMessageRequest.Message TypedTemplateMessage(TemplateMessageDto dto);

        WhatsAppMessageRequest.Message TypedTextMessage(TextMessageDto dto);
        WhatsAppMessageRequest.Message TypedTextMessageReply(TextMessageDto dto);
    }
}
