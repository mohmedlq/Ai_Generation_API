using DataAccess.Ai_Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class GeminiService : BaseAiService
    {
        private readonly Gemini_Ai_Api.GeminiOptions _options;

        public GeminiService(IOptions<Gemini_Ai_Api.GeminiOptions> options, HttpClient httpClient)
            : base(httpClient)
        {
            _options = options.Value;
        }

        public override async Task<string> GenerateAsync(string prompt, JsonElement schema, int? tokens = null, double? temperature = null)
        {
            var model = _options.Model;
            var apiKey = _options.ApiKey;

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

            var generationConfig = new Dictionary<string, object>
            {
                { "responseMimeType", "application/json" },
                { "maxOutputTokens", tokens ?? 4000 },
                { "temperature", temperature ?? 0.1 }
            };

            if (schema.ValueKind != JsonValueKind.Undefined && schema.ValueKind != JsonValueKind.Null)
            {
                generationConfig["responseSchema"] = schema;
            }

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = generationConfig
            };

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Add("x-goog-api-key", apiKey);
            requestMessage.Content = JsonContent.Create(requestBody);

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error ({(int)response.StatusCode}): {errorDetails}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonResponse);

            if (document.RootElement.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0 &&
                candidates[0].TryGetProperty("content", out var content) &&
                content.TryGetProperty("parts", out var parts) &&
                parts.GetArrayLength() > 0)
            {
                return parts[0].GetProperty("text").GetString() ?? string.Empty;
            }

            throw new Exception("Gemini API returned a response, but no text content was found.");
        }
    }
}