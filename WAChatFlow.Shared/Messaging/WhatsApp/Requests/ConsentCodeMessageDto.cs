namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class ConsentCodeMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Code { get; set; } = default!;
        public int ExpiresInMinutes { get; set; } = 5;
        public bool PreviewUrl { get; set; } = true;
    }
}
