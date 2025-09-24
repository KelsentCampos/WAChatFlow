namespace WAChatFlow.Shared.Messaging.WhatsApp.Requests
{
    public class TemplateMapDto
    {
        public class TemplateListItemDto
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Language { get; set; } = "";
            public string Status { get; set; } = "";
            public string Category { get; set; } = "";
            public int ComponentsCount { get; set; }
            public bool HasBody { get; set; }
            public bool HasButtons { get; set; }
        }

        public class TemplateListResponseDto
        {
            public List<TemplateListItemDto> Items { get; set; } = new();
            public string? Before { get; set; }
            public string? After { get; set; }
        }

        public class TemplateDetailDto
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Language { get; set; } = "";
            public string Status { get; set; } = "";
            public string Category { get; set; } = "";
            public string? SubCategory { get; set; }
            public string? ParameterFormat { get; set; }
            public int? MessageSendTtlSeconds { get; set; }
            public string? PreviousCategory { get; set; }
            public List<TemplateComponentDto> Components { get; set; } = new();
        }

        public class TemplateComponentDto
        {
            public string Type { get; set; } = "";
            public string? Format { get; set; }
            public string? Text { get; set; }
            public bool? AddSecurityRecommendation { get; set; }
            public int? CodeExpirationMinutes { get; set; }
            public List<List<string>>? BodyExamples { get; set; }
            public List<TemplateButtonDto>? Buttons { get; set; }
        }

        public class TemplateButtonDto
        {
            public string Type { get; set; } = "";
            public string? Text { get; set; }
            public string? Url { get; set; }
            public string? PhoneNumber { get; set; }
            public List<string>? Example { get; set; }
        }
    }
}
