using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        // Add other methods (POST, PUT, DELETE) as needed
    }
}