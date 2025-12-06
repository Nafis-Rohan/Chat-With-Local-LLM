using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BLL.External
{
    public static class OllamaClient
    {
        private static readonly HttpClient http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434") // Ollama default
        };

        public static async Task<string> GenerateAsync(string prompt)
        {
            var payload = new
            {
                model = "mistral",   // the model you pulled: mistral
                prompt = prompt,
                stream = false
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Ollama returns: { "response": "...", "model": "...", ... }
            var jo = JObject.Parse(responseString);
            return jo["response"]?.ToString() ?? "";
        }
    }
}
