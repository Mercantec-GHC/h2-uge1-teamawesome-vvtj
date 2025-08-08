using BlazorWASM.Components;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages;
public partial class CountryDashboard
{
	[Parameter]
	public string? CountryId { get; set; }
	[Parameter]
	public bool IsVisible { get; set; } = false;
	[Inject]
	NavigationManager NavigationManager { get; set; } = default!;
	[Inject]
	APIService APIService { get; set; } = default!;
	public CountryDataById? countryDashboard;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrEmpty(CountryId))
		{
			NavigateToList();
			return;
		}
		countryDashboard = await APIService.GetCountryDataByIdAsync(CountryId) ?? new CountryDataById();
		IsVisible = true;
	}
	public void NavigateToList()
	{
		var navUrl = $"/countries";
		NavigationManager.NavigateTo(navUrl);

	}
	public void NavigateToMain()
	{
		var navUrl = $"/";
		NavigationManager.NavigateTo(navUrl);

	}
}
