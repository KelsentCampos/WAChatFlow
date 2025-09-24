using System.Text.Json.Serialization;

namespace WAChatFlow.Shared.Messaging.WhatsApp.Responses
{
    public class WhatsAppResponse
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Message> Messages { get; set; }

        public class Contact
        {
            public string Input { get; set; }

            [JsonPropertyName("wa_id")]
            public string WaId { get; set; }
        }

        public class Message
        {
            public string Id { get; set; }
        }
    }
}
