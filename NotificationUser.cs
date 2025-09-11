namespace WAChatFlow.Shared.Models.Users
{
    public class NotificationUser
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string? FullName { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
