using System.Text.Json;
using System.Text.Json.Serialization;

namespace WAChatFlow.Shared.Messaging.WhatsApp.Responses
{
    public class WhatsAppTemplatesResponse
    {
        [JsonPropertyName("data")]
        public List<MessageTemplate> Data { get; set; } = new();

        [JsonPropertyName("paging")]
        public Paging? Paging { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("cursors")]
        public PagingCursors? Cursors { get; set; }
    }

    public class PagingCursors
    {
        [JsonPropertyName("before")]
        public string? Before { get; set; }

        [JsonPropertyName("after")]
        public string? After { get; set; }
    }

    public class MessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("parameter_format")]
        public string? ParameterFormat { get; set; }

        [JsonPropertyName("message_send_ttl_seconds")]
        public int? MessageSendTtlSeconds { get; set; }

        [JsonPropertyName("components")]
        public List<TemplateComponent> Components { get; set; } = new();

        [JsonPropertyName("language")]
        public string Language { get; set; } = default!; 

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("category")]
        public string Category { get; set; } = default!;

        [JsonPropertyName("sub_category")]
        public string? SubCategory { get; set; }

        [JsonPropertyName("previous_category")]
        public string? PreviousCategory { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Extra { get; set; }
    }

    public class TemplateComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Format { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }

        [JsonPropertyName("add_security_recommendation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AddSecurityRecommendation { get; set; }

        [JsonPropertyName("code_expiration_minutes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CodeExpirationMinutes { get; set; }

        [JsonPropertyName("example")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TemplateExample? Example { get; set; }

        [JsonPropertyName("buttons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<TemplateButton>? Buttons { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Extra { get; set; }
    }

    public class TemplateExample
    {
        [JsonPropertyName("body_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<List<string>>? BodyText { get; set; }

        [JsonPropertyName("header_text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? HeaderText { get; set; }

        [JsonPropertyName("header_handle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? HeaderHandle { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Extra { get; set; }
    }

    public class TemplateButton
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }

        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }

        [JsonPropertyName("example")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Example { get; set; }

        [JsonPropertyName("phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneNumber { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Extra { get; set; }
    }
}
