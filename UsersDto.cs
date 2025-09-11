using static WAChatFlow.Shared.Common.Enums;

namespace WAChatFlow.Shared.Models.Dtos
{
    public class UsersDto
    {
        public record UserStatusRow(string FullName, string PhoneNumber, ConsentStatus ConsentStatus, long UserId, string CommunicationChannel);
    }
}
