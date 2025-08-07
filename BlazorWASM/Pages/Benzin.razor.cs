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

    public List<DieselClass> listOfDiesel = new List<DieselClass>();
    public List<BenzinClass> listOfBenzin = new List<BenzinClass>();

    private List<DieselClass> FilteredList => listOfDiesel.Where(b => (!SelectedYear.HasValue || b.DateRange.Year == SelectedYear.Value) &&
                                                                      (!StartDate.HasValue || b.DateRange >= StartDate.Value) &&
                                                                      (!EndDate.HasValue || b.DateRange <= EndDate.Value) &&
                                                                      (!MinPrice.HasValue || b.PriceRange >= MinPrice.Value) &&
                                                                      (!MaxPrice.HasValue || b.PriceRange <= MaxPrice.Value)).ToList();

    protected override async Task OnInitializedAsync()
    {
        listOfDiesel = await APIService.GetDieselAsync();
        listOfBenzin = await APIService.GetBenzinAsync();
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
        var newListPrice_Diesel = new List<double?>();
        var newListPrice_Benzin = new List<double?>();

        var combinedDates = new HashSet<string>();

        foreach (var d in listOfDiesel)
        {
            combinedDates.Add(d.Date);
        }

        foreach (var b in listOfBenzin)
        {
            combinedDates.Add(b.Date);
        }

        var sortedDates = combinedDates.OrderBy(d => DateTime.Parse(d)).ToList();

        foreach (var i in listOfDiesel)
        {
            newListPrice_Diesel.Add(Convert.ToDouble(i.Price));
        }

        foreach (var i in listOfBenzin)
        {   
            newListPrice_Benzin.Add(Convert.ToDouble(i.Price));
        }

        Console.WriteLine();
        var data = new ChartData
        {
            Labels = sortedDates,
            Datasets = new List<IChartDataset>()
                {
                    new LineChartDataset()
                    {
                        Label = "Diesel prices over the years",
                        Data = newListPrice_Diesel,
                        BackgroundColor = "rgb(88, 80, 141)",
                        BorderColor = "rgb(88, 80, 141)",
                        BorderWidth = 2,
                        HoverBorderWidth = 4,
                        //PointBackgroundColor = "rgb(88, 80, 141)",
                        //PointBorderColor = "rgb(88, 80, 141)",
                        //PointRadius = 0, // hide points
                        //PointHoverRadius = 4,
                    },
                     new LineChartDataset()
                     {
                         Label = "Benzin prices over the years",
                         Data = newListPrice_Benzin,
                         BackgroundColor = "rgb(255, 166, 0)",
                         BorderColor = "rgb(255, 166, 0)",
                         BorderWidth = 2,
                         HoverBorderWidth = 4,
                         // PointBackgroundColor = "rgb(255, 166, 0)",
                         // PointBorderColor = "rgb(255, 166, 0)",
                         // PointRadius = 0, // hide points
                         // PointHoverRadius = 4,
                     }
                }
        };

        var options = new LineChartOptions();

        options.Interaction.Mode = InteractionMode.Index;

        options.Plugins.Title!.Text = "Diesel and Benzin";
        options.Plugins.Title.Display = true;
        options.Plugins.Title.Font = new ChartFont { Size = 20 };

        options.Responsive = true;

        options.Scales.X!.Title = new ChartAxesTitle { Text = "Dates", Display = true };
        options.Scales.Y!.Title = new ChartAxesTitle { Text = "Price", Display = true };

        await lineChart.InitializeAsync(data, options);
    }
}
    
