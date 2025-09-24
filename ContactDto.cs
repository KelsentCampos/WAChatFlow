using static WAChatFlow.Shared.Common.Enums;

namespace WAChatFlow.Shared.Contacts
{
    public class ContactDto
    {
        public record UserStatusRow(string FullName, string PhoneNumber, ConsentStatus ConsentStatus, long UserId, string CommunicationChannel);
    }
}
