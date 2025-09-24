using WAChatFlow.Shared.Contacts;
using static WAChatFlow.Shared.Contacts.ContactDto;

namespace WAChatFlow.Server.Infrastructure.Repositories.Contacts
{
    public interface IContactsRepository
    {
        Task<IEnumerable<UserStatusRow>> GetUserStatusCatalogAsync(CancellationToken ct = default);
        Task<NotificationUser> GetByIdAsync(long userId, CancellationToken ct = default);
        Task<NotificationUser> GetByPhoneAsync(string phoneNumber, CancellationToken ct = default);
        Task<long> UpsertByPhoneAsync(string phoneNumber, string fullName, CancellationToken ct = default);
    }
}
