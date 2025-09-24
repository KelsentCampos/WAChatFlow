namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class DocumentMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Caption { get; set; }
        public string? Filename { get; set; }
        public string? ReplyToMessageId { get; set; }
    }
}
