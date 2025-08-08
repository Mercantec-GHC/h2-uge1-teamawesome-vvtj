using BlazorBootstrap;
using BlazorWASM.Components;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages;
public partial class Countries
{
	[Inject]
	NavigationManager NavigationManager { get; set; } = default!;
	[Inject]
	APIService APIService { get; set; } = default!;
	public List<Country>? countries;

	protected override async Task OnInitializedAsync()
	{
		countries = await APIService.GetAllCountriesAsync() ?? new List<Country>();
	}

	private async Task<GridDataProviderResult<Country>> CountriesDataProvider(GridDataProviderRequest<Country> request)
	{
		return await Task.FromResult(request.ApplyTo(countries ?? Enumerable.Empty<Country>()));
	}

	public void ToggleViewButton(Country context)
	{
		if (context != null)
		{
			var country = (context as Country);
			var navUrl = $"/countries/{country.Id}";
			NavigationManager.NavigateTo(navUrl);
		}
	}
}
