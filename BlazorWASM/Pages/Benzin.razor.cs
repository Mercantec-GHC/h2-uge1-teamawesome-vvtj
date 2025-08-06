using BlazorBootstrap;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages;

public partial class Benzin : ComponentBase
{
    [Inject]
    private APIService ApiService { get; set; } = default!;

    private int? SelectedYear { get; set; }
    private DateTime? StartDate { get; set; }
    private DateTime? EndDate { get; set; }
    private decimal? MinPrice { get; set; }
    private decimal? MaxPrice { get; set; }

    private Modal modal = default!;
    private bool shouldRenderChart = false;

    public List<DieselClass> listAfBenzin = new List<DieselClass>();
    private List<DieselClass> FilteredList => listAfBenzin.Where(b => (!SelectedYear.HasValue || b.DateRange.Year == SelectedYear.Value) &&
                                                                      (!StartDate.HasValue || b.DateRange >= StartDate.Value) &&
                                                                      (!EndDate.HasValue || b.DateRange <= EndDate.Value) &&
                                                                      (!MinPrice.HasValue || b.PriceRange >= MinPrice.Value) &&
                                                                      (!MaxPrice.HasValue || b.PriceRange <= MaxPrice.Value)).ToList();

    protected override async Task OnInitializedAsync()
    {
        listAfBenzin = await APIService.GetOpgavesAsync();
        Console.WriteLine(listAfBenzin[0].Price);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (shouldRenderChart && lineChart != null)
        {
            shouldRenderChart = false;
            await RenderWormAsync();
        }
    }

    private async Task OnShowModalClick()
    {
        await modal.ShowAsync();
        shouldRenderChart = true;
    }

    // private async Task OnHideModalClick()
    // {
    //     await modal.HideAsync();
    // }
    
    private LineChart lineChart = default!;


    private async Task RenderWormAsync()
    {
        var newListDate = new List<string>();
        var newListPrice = new List<double?>();

        foreach (var date in listAfBenzin)
        {
            newListDate.Add(date.Date);
            newListPrice.Add(Convert.ToDouble(date.Price));

        }

        Console.WriteLine();
        var data = new ChartData
        {
            Labels = newListDate,
            Datasets = new List<IChartDataset>()
                {
                    new LineChartDataset()
                    {
                        Label = "Prices over the years",
                        Data = newListPrice,
                        BackgroundColor = "rgb(88, 80, 141)",
                        BorderColor = "rgb(88, 80, 141)",
                        BorderWidth = 2,
                        HoverBorderWidth = 4,
                        //PointBackgroundColor = "rgb(88, 80, 141)",
                        //PointBorderColor = "rgb(88, 80, 141)",
                        //PointRadius = 0, // hide points
                        //PointHoverRadius = 4,
                    }
                    // new LineChartDataset()
                    // {
                    //     Label = "England",
                    //     Data = new List<double?>{ 1, 1, 8, 19, 24, 26, 39, 47, 56, 66, 75, 88, 95, 100, 109, 114, 124, 129, 140, 142 },
                    //     BackgroundColor = "rgb(255, 166, 0)",
                    //     BorderColor = "rgb(255, 166, 0)",
                    //     BorderWidth = 2,
                    //     HoverBorderWidth = 4,
                    //     // PointBackgroundColor = "rgb(255, 166, 0)",
                    //     // PointBorderColor = "rgb(255, 166, 0)",
                    //     // PointRadius = 0, // hide points
                    //     // PointHoverRadius = 4,
                    // }
                }
        };

        var options = new LineChartOptions();

        options.Interaction.Mode = InteractionMode.Index;

        options.Plugins.Title!.Text = "Diesel";
        options.Plugins.Title.Display = true;
        options.Plugins.Title.Font = new ChartFont { Size = 20 };

        options.Responsive = true;

        options.Scales.X!.Title = new ChartAxesTitle { Text = "Dates", Display = true };
        options.Scales.Y!.Title = new ChartAxesTitle { Text = "Price", Display = true };

        await lineChart.InitializeAsync(data, options);
    }
}
    
