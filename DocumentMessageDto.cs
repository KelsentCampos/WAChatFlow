using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class DocumentMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Caption { get; set; }
        public string? Filename { get; set; }
        public string? ReplyToMessageId { get; set; }
    }
}
