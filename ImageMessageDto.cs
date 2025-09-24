namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class ImageMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Caption { get; set; }
        public string? ReplyToMessageId { get; set; }
    }
}
