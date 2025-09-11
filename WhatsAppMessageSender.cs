using WAChatFlow.Server.Configuration.WhatsApp;
using WAChatFlow.Shared.Interfaces.WhatsApp;
using Microsoft.Extensions.Options;
using WAChatFlow.Shared.Common;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp;
using WAChatFlow.Shared.Models.WhatsApp.Responses.WhatsApp.Error;
using WAChatFlow.Shared.Models.WhatsApp.Responses.Templates;
using WAChatFlow.Shared.Models.WhatsApp.Templates.DTOs;

namespace WAChatFlow.Server.Services.WhatsApp
{
    public class WhatsAppMessageSender : IWhatsAppMessageSender
    {
        private readonly ILogger<WhatsAppMessageSender> _logger;

        private readonly HttpClient _httpClient;
        private readonly WhatsAppCloudOptions _options;
        
        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public WhatsAppMessageSender(IHttpClientFactory httpClientFactory, ILogger<WhatsAppMessageSender> logger, IOptions<WhatsAppCloudOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;

            _options = options.Value;
        }

        public async Task<bool> ExecuteAsync(object model)
        {
            var json = JsonSerializer.Serialize(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_options.Endpoint}/{_options.ApiVersion}/{_options.PhoneNumberId}/messages";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            using var response = await _httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<WhatsAppResponse> ExecuteWithResultAsync(object model)
        {
            var url = $"{_options.Endpoint}/{_options.ApiVersion}/{_options.PhoneNumberId}/messages";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            var json = JsonSerializer.Serialize(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(url, content);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<WhatsAppResponse>(body, _jsonOptions);

            return result ?? new WhatsAppResponse();
        }

        public async Task<MetaSendResult<WhatsAppResponse>> ExecuteDetailedAsync(object model)
        {
            var url = $"{_options.Endpoint}/{_options.ApiVersion}/{_options.PhoneNumberId}/messages";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            var json = JsonSerializer.Serialize(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(url, content);

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<WhatsAppResponse>(body, _jsonOptions);

                return new MetaSendResult<WhatsAppResponse>
                {
                    IsSuccess = true,
                    Data = result,
                    StatusCode = response.StatusCode
                };
            }

            WhatsAppErrorResponse? error;

            try
            {
                error = JsonSerializer.Deserialize<WhatsAppErrorResponse>(body, _jsonOptions);
            }
            catch
            {
                error = new WhatsAppErrorResponse
                {
                    Error = new WhatsAppErrorResponse.ErrorBody { Message = body }
                };
            }

            if (error != null)
            {
                error.StatusCode = response.StatusCode;
            } 

            _logger.LogWarning(
                "Meta error HTTP {Status} code={Code} subcode={Sub} type={Type} details={Details} fbtrace={Trace}",
                (int)response.StatusCode,
                error?.Error?.Code,
                error?.Error?.ErrorSubcode,
                error?.Error?.Type,
                error?.Error?.ErrorData?.Details,
                error?.Error?.FbTraceId
            );

            return new MetaSendResult<WhatsAppResponse>
            {
                IsSuccess = false,
                Error = error,
                StatusCode = response.StatusCode
            };
        }

        public async Task<WhatsAppTemplatesResponse> GetTemplatesAsync()
        {
            var url = $"{_options.Endpoint}/{_options.ApiVersion}/{_options.WabaId}/message_templates";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            using var response = await _httpClient.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<WhatsAppTemplatesResponse>(body, _jsonOptions);

            return result ?? new WhatsAppTemplatesResponse();
        }

        public async Task<MessageTemplate> GetTemplateIdResponseAsyn(string id)
        {
            var url = $"{_options.Endpoint}/{_options.ApiVersion}/{id}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            using var response = await _httpClient.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<MessageTemplate>(body, _jsonOptions);

            return result ?? new MessageTemplate();
        }
    }
}
