using System.Text.Json.Serialization;

namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class WhatsAppMessageRequest
    {
        public class Message
        {
            [JsonPropertyName("messaging_product")]
            public string MessagingProduct { get; init; } = "whatsapp";

            [JsonPropertyName("recipient_type")]
            public string RecipientType { get; init; } = "individual";

            [JsonPropertyName("to")]
            public string To { get; init; }

            [JsonPropertyName("type")]
            public string Type { get; init; }

            [JsonPropertyName("context")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Context Context { get; init; }

            [JsonPropertyName("text")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Text Text { get; init; }

            [JsonPropertyName("image")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Media Image { get; init; }

            [JsonPropertyName("document")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Document Document { get; init; }

            [JsonPropertyName("location")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Location Location { get; init; }

            [JsonPropertyName("contacts")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Contact> Contacts { get; init; }

            [JsonPropertyName("interactive")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Interactive Interactive { get; init; }

            [JsonPropertyName("reaction")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Reaction Reaction { get; init; }

            [JsonPropertyName("template")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Template Template { get; init; }
        }

        public class Context
        {
            [JsonPropertyName("message_id")]
            public string MessageId { get; init; }
        }

        public class Text
        {
            [JsonPropertyName("body")]
            public string Body { get; init; }

            [JsonPropertyName("preview_url")]
            public bool PreviewUrl { get; init; } = false;
        }

        public class Media
        {
            [JsonPropertyName("link")]
            public string Link { get; init; }

            [JsonPropertyName("caption")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Caption { get; init; }
        }

        public class Document
        {
            [JsonPropertyName("link")]
            public string Link { get; init; }

            [JsonPropertyName("caption")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Caption { get; init; }

            [JsonPropertyName("filename")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Filename { get; init; }
        }

        public class Location
        {
            [JsonPropertyName("latitude")]
            public string Latitude { get; init; }

            [JsonPropertyName("longitude")]
            public string Longitude { get; init; }

            [JsonPropertyName("name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Name { get; init; }

            [JsonPropertyName("address")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Address { get; init; }
        }

        public class Contact
        {
            [JsonPropertyName("name")]
            public Name Name { get; init; }

            [JsonPropertyName("org")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Org Org { get; init; }

            [JsonPropertyName("phones")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Phone> Phones { get; init; }

            [JsonPropertyName("addresses")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Address> Addresses { get; init; }

            [JsonPropertyName("emails")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Email> Emails { get; init; }

            [JsonPropertyName("urls")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Url> Urls { get; init; }

            [JsonPropertyName("birthday")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Birthday { get; init; }
        }

        public class Name
        {
            [JsonPropertyName("formatted_name")]
            public string FormattedName { get; init; }

            [JsonPropertyName("first_name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string FirstName { get; init; }

            [JsonPropertyName("last_name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string LastName { get; init; }

            [JsonPropertyName("middle_name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string MiddleName { get; init; }

            [JsonPropertyName("suffix")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Suffix { get; init; }

            [JsonPropertyName("prefix")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Prefix { get; init; }
        }

        public class Org
        {
            [JsonPropertyName("company")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Company { get; init; }

            [JsonPropertyName("department")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Department { get; init; }

            [JsonPropertyName("title")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Title { get; init; }
        }

        public class Phone
        {
            [JsonPropertyName("phone")]
            public string PhoneNumber { get; init; }

            [JsonPropertyName("type")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Type { get; init; }

            [JsonPropertyName("wa_id")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string WaId { get; init; }
        }

        public class Address
        {
            [JsonPropertyName("street")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Street { get; init; }

            [JsonPropertyName("city")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string City { get; init; }

            [JsonPropertyName("state")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string State { get; init; }

            [JsonPropertyName("zip")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Zip { get; init; }

            [JsonPropertyName("country")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Country { get; init; }

            [JsonPropertyName("country_code")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string CountryCode { get; init; }

            [JsonPropertyName("type")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Type { get; init; }
        }

        public class Email
        {
            [JsonPropertyName("email")]
            public string EmailAddress { get; init; }

            [JsonPropertyName("type")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Type { get; init; }
        }

        public class Url
        {
            [JsonPropertyName("url")]
            public string UrlAddress { get; init; }

            [JsonPropertyName("type")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Type { get; init; }
        }

        public class Interactive
        {
            [JsonPropertyName("type")]
            public string Type { get; init; }

            [JsonPropertyName("header")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public InteractiveHeader? Header { get; init; }

            [JsonPropertyName("body")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public InteractiveBody Body { get; init; }

            [JsonPropertyName("footer")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public InteractiveFooter Footer { get; init; }

            [JsonPropertyName("action")]
            public InteractiveAction Action { get; init; }
        }

        public class InteractiveHeader
        {
            [JsonPropertyName("type")]
            public string Type { get; init; } = "text";

            [JsonPropertyName("text")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Text { get; init; }

            [JsonPropertyName("image")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public MediaLink? Image { get; init; }

            [JsonPropertyName("video")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public MediaLink? Video { get; init; }

            [JsonPropertyName("document")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public DocumentLink? Document { get; init; }
        }

        public class MediaLink
        {
            [JsonPropertyName("link")]
            public string Link { get; init; } = default!;
        }

        public class DocumentLink
        {
            [JsonPropertyName("link")]
            public string Link { get; init; } = default!;

            [JsonPropertyName("filename")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Filename { get; init; }
        }

        public class InteractiveBody
        {
            [JsonPropertyName("text")]
            public string Text { get; init; }
        }

        public class InteractiveFooter
        {
            [JsonPropertyName("text")]
            public string Text { get; init; }
        }

        public class InteractiveAction
        {
            [JsonPropertyName("button")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Button { get; init; }

            [JsonPropertyName("sections")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Section> Sections { get; init; }

            [JsonPropertyName("buttons")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<Button> Buttons { get; init; }

            [JsonPropertyName("name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Name { get; init; }

            [JsonPropertyName("parameters")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public CtaParameters Parameters { get; init; }
        }

        public class CtaParameters
        {
            [JsonPropertyName("display_text")]
            public string DisplayText { get; init; }

            [JsonPropertyName("url")]
            public string Url { get; init; }
        }

        public class Section
        {
            [JsonPropertyName("title")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Title { get; init; }

            [JsonPropertyName("rows")]
            public List<Row> Rows { get; init; }
        }

        public class Row
        {
            [JsonPropertyName("id")]
            public string Id { get; init; }

            [JsonPropertyName("title")]
            public string Title { get; init; }

            [JsonPropertyName("description")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Description { get; init; }
        }

        public class Button
        {
            [JsonPropertyName("type")]
            public string Type { get; init; } = "reply";

            [JsonPropertyName("reply")]
            public Reply Reply { get; init; }
        }

        public class Reply
        {
            [JsonPropertyName("id")]
            public string Id { get; init; }

            [JsonPropertyName("title")]
            public string Title { get; init; }
        }

        public class Reaction
        {
            [JsonPropertyName("message_id")]
            public string MessageId { get; init; }

            [JsonPropertyName("emoji")]
            public string Emoji { get; init; }
        }

        public class Template
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("language")]
            public TemplateLanguage Language { get; init; }

            [JsonPropertyName("components")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<TemplateComponent> Components { get; init; }
        }

        public class TemplateLanguage
        {
            [JsonPropertyName("code")]
            public string Code { get; init; }
        }

        public class TemplateComponent
        {
            [JsonPropertyName("type")]
            public string Type { get; init; } = default!;

            [JsonPropertyName("sub_type")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? SubType { get; init; } 

            [JsonPropertyName("index")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Index { get; init; } 

            [JsonPropertyName("parameters")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public List<TemplateParameter>? Parameters { get; init; }
        }

        public class TemplateParameter
        {
            [JsonPropertyName("type")]
            public string Type { get; init; } = default!;

            [JsonPropertyName("text")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Text { get; init; }

            [JsonPropertyName("payload")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Payload { get; init; }

            [JsonPropertyName("phone_number")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? PhoneNumber { get; init; }

            [JsonPropertyName("currency")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public CurrencyParam? Currency { get; init; }

            [JsonPropertyName("date_time")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public DateTimeParam? DateTime { get; init; }

            [JsonPropertyName("image")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public MediaParam? Image { get; init; }

            [JsonPropertyName("video")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public MediaParam? Video { get; init; }

            [JsonPropertyName("document")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public DocumentParam? Document { get; init; }
        }

        public class CurrencyParam
        {
            [JsonPropertyName("fallback_value")] public string FallbackValue { get; init; } = default!;
            [JsonPropertyName("code")] public string Code { get; init; } = default!; 
            [JsonPropertyName("amount_1000")] public int Amount1000 { get; init; } 
        }

        public class DateTimeParam
        {
            [JsonPropertyName("fallback_value")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? FallbackValue { get; init; }

            [JsonPropertyName("day_of_week")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? DayOfWeek { get; init; }
            [JsonPropertyName("year")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? Year { get; init; }
            [JsonPropertyName("month")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? Month { get; init; }
            [JsonPropertyName("day_of_month")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? DayOfMonth { get; init; }
            [JsonPropertyName("hour")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? Hour { get; init; }
            [JsonPropertyName("minute")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? Minute { get; init; }
            [JsonPropertyName("calendar")][JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Calendar { get; init; }
        }

        public class MediaParam
        {
            [JsonPropertyName("link")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Link { get; init; }

            [JsonPropertyName("id")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Id { get; init; }
        }

        public class DocumentParam : MediaParam
        {
            [JsonPropertyName("filename")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Filename { get; init; }
        }

    }
}