namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class ReplyButtonsMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string BodyText { get; set; } = default!;
        public List<ReplyButtonDto> Buttons { get; set; } = new();
        public string? FooterText { get; set; }
        public string? ReplyToMessageId { get; set; }
    }

    public class ReplyButtonDto
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
    }
}
