namespace WAChatFlow.Server.Infrastructure.Repositories.Consent
{
    public interface IConsentRepository
    {
        Task<long> CreateAsync(long userId, string channel, string code, TimeSpan ttl, CancellationToken ct = default);

        Task<bool> VerifyAndConsumeAsync(long userId, string channel, string code);

        Task MarkConsentValidatedAsync(long userId, string channel, string consentGrantedMessageId);
    }
}
