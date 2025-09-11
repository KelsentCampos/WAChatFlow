using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class MarkAsReadDto
    {
        public string MessageId { get; set; } = default!;
    }
}
