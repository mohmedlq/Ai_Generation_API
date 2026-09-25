using DataAccess.Groq_Ai_Api;
using DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ai_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiApiController : ControllerBase
    {
        private readonly GeminiService _geminiService;
        private readonly GroqService _groqService;

        public AiApiController(GeminiService geminiService,GroqService groqService)
        {
            _geminiService = geminiService;
            _groqService = groqService;
        }

        [HttpPost("generate/Gemini")]
        public async Task<IActionResult> GeminiGenerator([FromBody] Dtos.AiGenerateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new
                {
                    error = "Prompt cannot be empty."
                });
            }

            try
            {
                var result = await _geminiService.GenerateAsync(
                    request.Prompt,
                    request.Schema,
                    request.Tokens,
                    request.Temperature
                );

                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }
        
        [HttpPost("generate/Groq")]
        public async Task<IActionResult> GroqGenerator([FromBody] Dtos.AiGenerateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new
                {
                    error = "Prompt cannot be empty."
                });
            }

            try
            {
                var result = await _groqService.GenerateAsync(
                    request.Prompt,
                    request.Schema,
                    request.Tokens,
                    request.Temperature
                );

                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

    }
}