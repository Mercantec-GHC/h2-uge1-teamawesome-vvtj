using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWASM.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opgaver.mercantec.tech";

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BackendStatus?> GetBackendStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/api/Status/all");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<BackendStatus>(jsonString, options);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved tjek af backend status: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Benzin>> GetBenzinAsync()
        {
            var response = await _httpClient.GetAsync("https://opgaver.mercantec.tech/Opgaver/Miles95");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<Benzin>>();
            return results ?? Enumerable.Empty<Benzin>();
        }
    }

    public class BackendStatus
    {
        public ServerStatus? Server { get; set; }
        public DatabaseStatus? MongoDB { get; set; }
        public DatabaseStatus? PostgreSQL { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public class Benzin
    {
        public string Price { get; set; } = "0";
        public string Date { get; set; } = "00.00";
    }

    public class ServerStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    public class DatabaseStatus
    {
        public string Status { get; set; } = string.Empty;
        public string? Database { get; set; }
        public string? Error { get; set; }
        public bool IsError { get; set; }
    }
}
