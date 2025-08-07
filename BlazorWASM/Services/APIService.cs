using System.ComponentModel;
using System.Text.Json;
using BlazorWASM.Pages;


namespace BlazorWASM.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opgaver.mercantec.tech";
        private const string BaseUrl_2 = "https://opgaver.mercantec.tech";

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

        public async Task<List<DieselClass>> GetDieselAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/Opgaver/diesel");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<DieselClass>>(json);
                }
                else
                {
                    Console.WriteLine($"Fejl: {response.StatusCode}");
                    return new List<DieselClass>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Netværksfejl: {ex.Message}");
                return new List<DieselClass>();
            }
        }

        public async Task<List<BenzinClass>> GetBenzinAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl_2}/Opgaver/Miles95");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<BenzinClass>>(json);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<BenzinClass>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Networks error: {ex.Message}");
                return new List<BenzinClass>();
            }
        }
    }


    public class BackendStatus
    {
        public ServerStatus? Server { get; set; }
        public DatabaseStatus? MongoDB { get; set; }
        public DatabaseStatus? PostgreSQL { get; set; }
        public DateTime Timestamp { get; set; }
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

    public class DieselClass
    {
        public string? Price { get; set; }
        public string? Date { get; set; }

        //Ternary
        public decimal PriceRange => decimal.TryParse(Price, out var p) ? p : 0;
        public DateTime DateRange => DateTime.TryParse(Date, out var d) ? d : DateTime.MinValue;
    }

    public class BenzinClass
    {
        public string? Price { get; set; }
        public string? Date { get; set; }

    }
}
