namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class ReactionReplyDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string ReactToMessageId { get; set; } = default!;
        public string Emoji { get; set; } = "👍";
    }
}
