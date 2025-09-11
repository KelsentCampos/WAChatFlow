namespace WAChatFlow.Shared.Models.WhatsApp.Requests.DTOs
{
    public class ContactsMessageDto
    {
        public string PhoneNumber { get; set; } = default!;
        public List<ContactItemDto> Contacts { get; set; } = new();
        public string? ReplyToMessageId { get; set; }
    }

    public class ContactItemDto
    {
        public string FullName { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }

        public string? Company { get; set; }
        public string? Department { get; set; }
        public string? Title { get; set; }

        public List<ContactPhoneDto>? Phones { get; set; } = new();

        public List<ContactEmailDto>? Emails { get; set; } = new();

        public List<ContactAddressDto>? Addresses { get; set; } = new();

        public List<ContactUrlDto>? Urls { get; set; } = new();

        public string? Birthday { get; set; }
    }

    public class ContactPhoneDto
    {
        public string? Phone { get; set; } 
        public string? WaId { get; set; }
        public string? Type { get; set; }  
    }

    public class ContactEmailDto
    {
        public string? Email { get; set; }
        public string? Type { get; set; }
    }

    public class ContactAddressDto
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? Type { get; set; }
    }

    public class ContactUrlDto
    {
        public string? Url { get; set; }
        public string? Type { get; set; } 
    }
}
