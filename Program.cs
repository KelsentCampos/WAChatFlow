using Microsoft.AspNetCore.Http.Features;
using WAChatFlow.Server.Configuration.OpenAI;
using WAChatFlow.Server.Configuration.Webhook;
using WAChatFlow.Server.Configuration.WhatsApp;
using WAChatFlow.Server.Repositories.DataBase;
using WAChatFlow.Server.Repositories.Users;
using WAChatFlow.Server.Services.WhatsApp;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Interfaces.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Requests;
using WAChatFlow.Shared.Models.WhatsApp.Requests.Builders;
using WAChatFlow.Shared.Models.WhatsApp.Responses.Templates;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp.Error;
using WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs;
using WAChatFlow.Shared.Models.WhatsApp.Webhook;
using WAChatFlow.Server.Mapping;

var builder = WebApplication.CreateBuilder(args);

// APIs
builder.Services.AddControllers();

// Infra básica
builder.Services.AddHttpClient();

// WhatsApp
builder.Services.AddScoped<IWhatsAppMessageSender, WhatsAppMessageSender>();
builder.Services.AddSingleton<IWhatsAppRequestBuilder, WhatsAppRequestBuilder>();
builder.Services.AddSingleton<ITemplateMappings, TemplateMappings>();

// Usuario 
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// POCOs inyectados en WhatsAppWebhookController (si los dejas en el ctor)
builder.Services.AddTransient<WhatsAppWebhookEvent>();
builder.Services.AddTransient<WhatsAppMessageRequest>();
builder.Services.AddTransient<WhatsAppResponse>();
builder.Services.AddTransient<WhatsAppErrorResponse>();
builder.Services.AddTransient<WhatsAppTemplatesResponse>();

// Configuración de Base de Datos
builder.Services.AddSingleton<DatabaseConnection>();
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
