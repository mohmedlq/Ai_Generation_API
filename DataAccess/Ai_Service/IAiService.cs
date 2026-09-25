using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Ai_Service
{
    public interface IAiService
    {
        Task<string>  GenerateAsync(string prompt, JsonElement schema, int? tokens = null, double? temperature = null);

    }
}
