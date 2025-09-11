namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class ReactionReplyDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string ReactToMessageId { get; set; } = default!;
        public string Emoji { get; set; } = "👍";
    }
}
