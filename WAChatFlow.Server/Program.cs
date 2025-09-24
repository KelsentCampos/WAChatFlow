using Microsoft.AspNetCore.Http.Features;

using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Common.Utilities;
using WAChatFlow.Shared.Contacts;
using WAChatFlow.Shared.Messaging;
using WAChatFlow.Shared.Messaging.Webhook;
using WAChatFlow.Shared.Messaging.WhatsApp;
using WAChatFlow.Shared.Messaging.WhatsApp.Requests;
using WAChatFlow.Shared.Messaging.WhatsApp.Responses;

using WAChatFlow.Server.Configuration;
using WAChatFlow.Server.Configuration.Options;
using WAChatFlow.Server.Controllers.Contacts;
using WAChatFlow.Server.Controllers.Messaging;
using WAChatFlow.Server.Controllers.Templates;
using WAChatFlow.Server.Controllers.Webhooks;
using WAChatFlow.Server.Infrastructure.Data;
using WAChatFlow.Server.Infrastructure.Repositories;
using WAChatFlow.Server.Infrastructure.Repositories.Contacts;
using WAChatFlow.Server.Services;
using WAChatFlow.Server.Services.Messaging;
using WAChatFlow.Server.Services.Messaging.Senders;
using WAChatFlow.Server.Services.Messaging.Builders;
using WAChatFlow.Server.Services.Templates;
using Microsoft.Data.SqlClient;
using WAChatFlow.Server.Infrastructure.Repositories.Consent;

var builder = WebApplication.CreateBuilder(args);

// APIs
builder.Services.AddControllers();

// Infra básica
builder.Services.AddHttpClient();

// WhatsApp
builder.Services.AddScoped<IWhatsAppMessageSender, WhatsAppMessageSender>();
builder.Services.AddSingleton<IWhatsAppRequestBuilder, WhatsAppRequestBuilder>();
builder.Services.AddSingleton<ITemplateMappings, TemplateMappings>();

// Repositorios
builder.Services.AddScoped<IConsentRepository, ConsentRepository>();
builder.Services.AddScoped<IContactsRepository, ContactsRepository>();

// POCOs inyectados en WhatsAppWebhookController (si los dejas en el ctor)
builder.Services.AddTransient<WhatsAppWebhookEvent>();
builder.Services.AddTransient<WhatsAppMessageRequest>();
builder.Services.AddTransient<WhatsAppResponse>();
builder.Services.AddTransient<WhatsAppErrorResponse>();
builder.Services.AddTransient<WhatsAppTemplatesResponse>();

// Configuración de Base de Datos
builder.Services.AddSingleton<DatabaseConnectionFactory>();

// Options
builder.Services.Configure<WhatsAppCloudOptions>(builder.Configuration.GetSection("WhatsAppCloud"));
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));
builder.Services.Configure<WebhookVerificationOptions>(builder.Configuration.GetSection("WebhookVerification"));

// Uploads grandes
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = Constants.Bytes.GiB;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Servir WASM (Client) + estáticos
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Endpoints
app.MapControllers();

// SPA fallback al index.html del Client
app.MapFallbackToFile("index.html");

app.Run();
