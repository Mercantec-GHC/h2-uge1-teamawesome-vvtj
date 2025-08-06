using System.Text.Json;
using Azure;
using static BlazorWASM.Pages.MyAPIProject;

namespace BlazorWASM.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opgaver.mercantec.tech/";

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BackendStatus?> GetBackendStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}api/Status/all");

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

		public async Task<List<GasPrice>?> GetGasPricesAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync($"{BaseUrl}Opgaver/Miles95");

				if (response.IsSuccessStatusCode)
				{
					var jsonString = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<GasPrice>?>(jsonString);
				}

				else
				{
					Console.WriteLine($"Error: {response.StatusCode}");
					return new List<GasPrice>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex}");
				return null;
			}
		}

		public async Task<List<DieselPrice>?> GetDieselPricesAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync($"{BaseUrl}Opgaver/Diesel");

				if (response.IsSuccessStatusCode)
				{
					var jsonString = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<DieselPrice>?>(jsonString);
				}

				else
				{
					Console.WriteLine($"Error: {response.StatusCode}");
					return new List<DieselPrice>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex}");
				return null;
			}
		}
		//public async Task<List<DieselPrice>?> GetDieselPricesAsync()
		//{
		//	try
		//	{
		//		var response = await _httpClient.GetAsync($"{BaseUrl}Countries");

		//		if (response.IsSuccessStatusCode)
		//		{
		//			var jsonString = await response.Content.ReadAsStringAsync();
		//			return JsonSerializer.Deserialize<List<DieselPrice>?>(jsonString);
		//		}

		//		else
		//		{
		//			Console.WriteLine($"Error: {response.StatusCode}");
		//			return new List<DieselPrice>();
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error: {ex}");
		//		return null;
		//	}
		//}
	}
	public class Countries
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Capital { get; set; }
		public int Population { get; set; }

	}

	
	public class GasPrice
	{
		public string? Date { get; set; }
		public string? Price { get; set; }
	}

	public class DieselPrice
	{
		public string? Date { get; set; }
		public string? Price { get; set; }
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
}
