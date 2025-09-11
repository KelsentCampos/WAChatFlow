using System.Text.Json.Serialization;

namespace WAChatFlow.Shared.Models.WhatsApp.Webhook
{
    public class WhatsAppWebhookEvent
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<Entry> Entry { get; set; }
    }

    public class Entry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<Change> Changes { get; set; }
    }

    public class Change
    {
        [JsonPropertyName("field")]
        public string Field { get; set; } 

        [JsonPropertyName("value")]
        public Value Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("statuses")]
        public List<Status> Statuses { get; set; }

        // En fallos algunos providers anidan "errors" aquí o dentro de statuses[i].errors
        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class Contact
    {
        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }

        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } // text, image, document, location, interactive, reaction, system, etc.

        // Text
        [JsonPropertyName("text")]
        public TextObject Text { get; set; }

        // Media (referenciadas por id cuando el usuario envía)
        [JsonPropertyName("image")]
        public MediaObject Image { get; set; }

        [JsonPropertyName("document")]
        public DocumentObject Document { get; set; }

        [JsonPropertyName("location")]
        public LocationObject Location { get; set; }

        // Interacciones: botones/lista de respuestas
        [JsonPropertyName("button")]
        public ButtonObject Button { get; set; } // para mensajes “button” antiguos

        [JsonPropertyName("interactive")]
        public InteractiveObject Interactive { get; set; }

        // Contexto de reply
        [JsonPropertyName("context")]
        public Context Context { get; set; }

        // Reacciones del usuario
        [JsonPropertyName("reaction")]
        public Reaction Reaction { get; set; }
    }

    public class TextObject
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class MediaObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }
    }

    public class DocumentObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }
    }

    public class LocationObject
    {
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class ButtonObject
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }

    public class InteractiveObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } // "button" o "list_reply"

        [JsonPropertyName("button_reply")]
        public ButtonReply ButtonReply { get; set; }

        [JsonPropertyName("list_reply")]
        public ListReply ListReply { get; set; }
    }

    public class ButtonReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class ListReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Context
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Reaction
    {
        [JsonPropertyName("emoji")]
        public string Emoji { get; set; }

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } // mensaje (wamid.*)

        [JsonPropertyName("status")]
        public string StatusValue { get; set; } // delivered, read, failed, etc.

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; }

        [JsonPropertyName("conversation")]
        public Conversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public Pricing Pricing { get; set; }

        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }
    }

    public class Conversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public Origin Origin { get; set; }
    }

    public class Origin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } // user_initiated, business_initiated, referral_conversion
    }

    public class Pricing
    {
        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; } // CBP

        [JsonPropertyName("category")]
        public string Category { get; set; } // utility, marketing, authentication, service
    }

    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }
}
