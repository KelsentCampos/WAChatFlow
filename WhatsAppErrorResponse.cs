using System.Text.Json.Serialization;
using System.Net;

namespace WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp.Error
{
    public class WhatsAppErrorResponse
    {
        [JsonPropertyName("error")]
        public ErrorBody? Error { get; set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public class ErrorBody
        {
            [JsonPropertyName("message")]
            public string? Message { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; } 

            [JsonPropertyName("code")]
            public int? Code { get; set; } 

            [JsonPropertyName("error_subcode")]
            public int? ErrorSubcode { get; set; }

            [JsonPropertyName("error_user_title")]
            public string? ErrorUserTitle { get; set; }

            [JsonPropertyName("error_user_msg")]
            public string? ErrorUserMsg { get; set; }

            [JsonPropertyName("fbtrace_id")]
            public string? FbTraceId { get; set; }

            [JsonPropertyName("error_data")]
            public ErrorData? ErrorData { get; set; }
        }

        public class ErrorData
        {
            [JsonPropertyName("messaging_product")]
            public string? MessagingProduct { get; set; }

            [JsonPropertyName("details")]
            public string? Details { get; set; }
        }
    }
}
