using System.Text.Json.Serialization;

namespace BlazorWASM.Components;
public class Country
{
	public string? Id { get; set; }
	public CountryName Name { get; set; } = new CountryName();
	public string? Flag { get; set; }
	public string? Region { get; set; }
	public string? Continent { get; set; }
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
	public int Population { get; set; }
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