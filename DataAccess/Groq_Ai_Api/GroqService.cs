using DataAccess.Ai_Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Groq_Ai_Api
{
    public class GroqService : BaseAiService
    {
        private readonly Groq_Ai_Api.GroqOptions _options;
        public GroqService(IOptions<Groq_Ai_Api.GroqOptions>Option,HttpClient httpClient):base(httpClient)
        {
            _options = Option.Value;
        }
        public override async Task<string> GenerateAsync(string prompt, JsonElement schema, int? tokens = null, double? temperature = null)
        {
            var model = _options.Model;
            var apiKey = _options.ApiKey;

            // رابط Groq المتوافق مع معايير OpenAI
            var url = "https://api.groq.com/openai/v1/chat/completions";

            // تجهيز قائمة الرسائل
            var messages = new List<object>();

            // إذا كان هناك Schema، نمررها كتعليمة صارمة للنظام ليعيد JSON مطابق
            if (schema.ValueKind != JsonValueKind.Undefined && schema.ValueKind != JsonValueKind.Null)
            {
                messages.Add(new
                {
                    role = "system",
                    content = $"You are a helpful assistant. You must output your response exactly in JSON format that strictly matches the following schema: {schema.GetRawText()}"
                });
            }

            // إضافة رسالة المستخدم
            messages.Add(new
            {
                role = "user",
                content = prompt
            });

            // بناء هيكل الطلب لـ Groq
            var requestBody = new
            {
                model = model,
                messages = messages,
                temperature = temperature ?? 0.1,
                max_tokens = tokens ?? 4000,
                // إجبار الموديل على إرجاع كائن JSON
                response_format = new { type = "json_object" }
            };

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            // لاحظ أن Groq يستخدم Bearer Token بدلاً من x-goog-api-key
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            requestMessage.Content = JsonContent.Create(requestBody);

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq API Error ({(int)response.StatusCode}): {errorDetails}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonResponse);

            // القراءة الآمنة للنتيجة بناءً على هيكل استجابة Groq / OpenAI
            if (document.RootElement.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0 &&
                choices[0].TryGetProperty("message", out var message) &&
                message.TryGetProperty("content", out var content))
            {
                return content.GetString() ?? string.Empty;
            }

            throw new Exception("Groq API returned a response, but no text content was found.");
        }
    }
}
