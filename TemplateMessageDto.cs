namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class TemplateMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string TemplateName { get; set; } = default!;
        public string LanguageCode { get; set; } = "es_MX";
        public IEnumerable<object>? Components { get; set; }
    }
}
