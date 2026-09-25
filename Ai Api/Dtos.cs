using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Ai_Api
{
    public class Dtos
    {
        public class AiGenerateRequest
        {
            [Required]
            public string Prompt { get; set; } = string.Empty;

            public JsonElement Schema { get; set; } = new();

            public int? Tokens {  get; set; }

            public double? Temperature { get; set; }
        }

    }
}
