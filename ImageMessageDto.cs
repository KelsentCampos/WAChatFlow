namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class ImageMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Caption { get; set; }
        public string? ReplyToMessageId { get; set; }
    }
}
