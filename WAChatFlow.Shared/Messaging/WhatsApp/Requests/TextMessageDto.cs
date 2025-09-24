namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class TextMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Body { get; set; } = default!;
        public bool PreviewUrl { get; set; } = false;
        public string? ReplyToMessageId { get; set; }
    }
}
