namespace WAChatFlow.Shared.Consent
{
    public class VerifyConsentDto
    {
        public long UserId { get; set; }
        public string Code { get; set; } = default!;
        public string Wamid { get; set; } = default!;
    }
}
