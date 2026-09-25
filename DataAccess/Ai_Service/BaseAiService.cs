using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Ai_Service
{
    public abstract class BaseAiService : IAiService
    {

        protected readonly HttpClient _httpClient;
        public BaseAiService(
             HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public abstract Task<string> GenerateAsync(string prompt, JsonElement schema, int? tokens = null, double? temperature = null);
        
    }
}
