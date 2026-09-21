using DataAccess.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace DataAccess.Services
{
    public class GeminiService:IGeminiService
    {
        private readonly GeminiOptions _options;
        private readonly HttpClient _httpClient;

        public GeminiService(IOptions<GeminiOptions> options,
             HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;

        }
        public async Task<string> GenerateAsync(
            string prompt,
            JsonElement schema,
            int? maxTokens = null)
        {
            var model = _options.Model;
            var apiKey = _options.ApiKey;

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

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

                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = schema,
                    maxOutputTokens = maxTokens ?? 4000,
                    temperature = 0.1
                }
            };
            
            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, url);

            requestMessage.Headers.Add("x-goog-api-key", apiKey);

            requestMessage.Content =
                JsonContent.Create(requestBody);

            var response =
                await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails =
                    await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Gemini API Error ({(int)response.StatusCode}): {errorDetails}");
            }

            var jsonResponse =
                await response.Content.ReadAsStringAsync();

            using var document =
                JsonDocument.Parse(jsonResponse);

            var actualText = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return actualText ?? string.Empty;
        }

    }
}
