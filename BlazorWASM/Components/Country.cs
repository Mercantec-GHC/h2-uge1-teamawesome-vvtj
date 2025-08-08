using System.Text.Json.Serialization;

namespace BlazorWASM.Components;
public class Country
{
	public string? Id { get; set; }
	public CountryName Name { get; set; } = new CountryName();
	public string? OfficialName => Name?.Official;
	public string? Region { get; set; }
	public string? Subregion { get; set; }
	public List<string>? Continents { get; set; } = new List<string>();
}

public class CountryName
{
	public string? Common { get; set; }
	public string? Official { get; set; }
}

public class Flag
{
	public string? Png { get; set; }
	public string? Svg { get; set; }
}

public class CountryDataById
{
	public string? Id { get; set; }
	public CountryName Name { get; set; } = new CountryName();
	public Flag Flags { get; set; } = new();
	public List<string>? Capital { get; set; } = new List<string>();
	public Dictionary<string, Currency> Currencies { get; set; } = new Dictionary<string, Currency>();
	public Dictionary<string, string> Languages { get; set; } = new Dictionary<string, string>();
	public int? Population { get; set; } 
	public double? Area { get; set; } 
}

public class Currency
{
	[JsonPropertyName("name")]
	public string? CurrencyName { get; set; }
}

public class Language
{
	[JsonPropertyName("name")]
	public string? LanguageName { get; set; }
}