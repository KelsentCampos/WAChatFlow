using WAChatFlow.Shared.Models.Users;
using static WAChatFlow.Shared.Models.Dtos.UsersDto;

namespace WAChatFlow.Server.Repositories.Users
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserStatusRow>> GetUserStatusCatalogAsync(CancellationToken ct = default);
        Task<NotificationUser> GetByIdAsync(long userId, CancellationToken ct = default);
        Task<NotificationUser> GetByPhoneAsync(string phoneNumber, CancellationToken ct = default);
        Task<long> UpsertByPhoneAsync(string phoneNumber, string fullName, CancellationToken ct = default);
    }
}
