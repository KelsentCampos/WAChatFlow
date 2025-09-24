namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class LocationMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ReplyToMessageId { get; set; }
    }
}
