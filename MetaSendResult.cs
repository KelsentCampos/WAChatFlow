using System.Net;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp.Error;

namespace WAChatFlow.Shared.Common
{
    public class MetaSendResult<TSuccess>
    {
        public bool IsSuccess { get; init; }
        public TSuccess? Data { get; init; }
        public WhatsAppErrorResponse? Error { get; init; }
        public HttpStatusCode StatusCode { get; init; }
    }
}
