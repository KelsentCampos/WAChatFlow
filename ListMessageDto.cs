namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class ListMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string? HeaderText { get; set; }
        public string BodyText { get; set; } = default!;
        public string ButtonText { get; set; } = default!;
        public List<ListSectionDto> Sections { get; set; } = new();
        public string? FooterText { get; set; }
        public string? ReplyToMessageId { get; set; }
    }

    public class ListSectionDto
    {
        public string Title { get; set; } = default!;
        public List<ListRowDto> Rows { get; set; } = new();
    }

    public class ListRowDto
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }
}
