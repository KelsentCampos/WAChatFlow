namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class AudioMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? ReplyToMessageId { get; set; }
    }
}
