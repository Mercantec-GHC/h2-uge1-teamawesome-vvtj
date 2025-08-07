using BlazorBootstrap;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages
{
    public partial class Opgave
    {
        [Inject]
        APIService _apiService { get; set; } = default;

        private LineChart lineChart = default!;
        private LineChartOptions lineChartOptions = default!;
        private ChartData chartData = default!;
        private IEnumerable<FuelViewModel> data = default!;
        private List<Fuel>? benzinData;
        private List<Fuel>? diezelData;
        private string _Warning = "Server not working, we try to find a solution, thanks for yours waiting";
        private bool isSuccess = true;

        private async Task GetData()
        {
            var benzinResult = await _apiService.GetBenzinAsync();
            var diezelResult = await _apiService.GetDiezelAsync();

            benzinData = benzinResult?.ToList();
            diezelData = diezelResult?.ToList();

            if (benzinData == null || diezelData == null)
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }
        }

        private async Task LoadData()
        {
            await GetData();
            if (isSuccess)
            {

                data = await GetFuelDataAsync();
                await BuildChartAsync();
            }
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadData();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task BuildChartAsync()
        {
            var colors = ColorUtility.CategoricalTwelveColors;

            var benzinLabels = benzinData.Select(x => x.Date).ToList();
            var benzinValues = benzinData.Select(x => (double?)x.Price).ToList();
            var diezelValues = diezelData.Select(x => (double?)x.Price).ToList();

            chartData = new ChartData
            {
                Labels = benzinLabels,
                Datasets = new List<IChartDataset>
                {
                    new LineChartDataset
                    {
                        Label = "Benzin Pris",
                        Data = benzinValues,
                        BackgroundColor = colors[0],
                        BorderColor = colors[0],
                        BorderWidth = 2,
                        HoverBorderWidth = 4
                    },

                    new LineChartDataset
                    {
                        Label = "Diezel Pris",
                        Data = diezelValues,
                        BackgroundColor = colors[1],
                        BorderColor = colors[1],
                        BorderWidth = 2,
                        HoverBorderWidth = 4
                    }
                }
            };

            lineChartOptions = new LineChartOptions
            {
                Responsive = true,
                Interaction = new Interaction { Mode = InteractionMode.Index },
                Scales = new()
                {
                    X = new() { Title = new ChartAxesTitle { Text = "Dato", Display = true } },
                    Y = new() { Title = new ChartAxesTitle { Text = "Pris", Display = true } }
                },
            };

            await lineChart.InitializeAsync(chartData, lineChartOptions);
        }

        private async Task<GridDataProviderResult<FuelViewModel>> DataProvider(GridDataProviderRequest<FuelViewModel> request)
        {
            await GetData();
            if (data is null) // pull data only once for client-side filtering, sorting, and paging
                data = await GetFuelDataAsync(); // async API call

            return request.ApplyTo(data);
        }

        public async Task<IEnumerable<FuelViewModel>> GetFuelDataAsync()
        {
            return await GetFuelPrice();
        }

        private async Task<IEnumerable<FuelViewModel>> GetFuelPrice()
        {
            FuelViewModel benzinView = new FuelViewModel()
            {
                FuelType = "Benzin",
                LastPrice = benzinData.Last().Price,
                AvaragePrice = benzinData.Average(x => x.Price),
                Mediana = CalculateMedian(benzinData.Select(x => x.Price)),
                Variance = CalculateVariance(benzinData.Select(x => x.Price)),
            };

            FuelViewModel diezelView = new FuelViewModel()
            {
                FuelType = "Diesel",
                LastPrice = diezelData.Last().Price,
                AvaragePrice = diezelData.Average(x => x.Price),
                Mediana = CalculateMedian(diezelData.Select(x => x.Price)),
                Variance = CalculateVariance(diezelData.Select(x => x.Price)),
            };

            return new List<FuelViewModel>
            {
                benzinView,
                diezelView
            };
        }

        public double CalculateMedian(IEnumerable<double> values)
        {
            if (values == null)
                return 0;

            var sorted = values.OrderBy(x => x).ToList();
            int count = sorted.Count;
            int mid = count / 2;

            return count % 2 == 0
                ? (sorted[mid - 1] + sorted[mid]) / 2.0
                : sorted[mid];
        }

        public double CalculateVariance(IEnumerable<double> values)
        {
            if (values == null)
                return 0;

            double average = values.Average();
            double variance = values.Sum(x => Math.Pow(x - average, 2)) / values.Count();

            return variance;
        }

        public class FuelViewModel
        {
            public string? FuelType { get; set; }
            public double LastPrice { get; set; }
            public double AvaragePrice { get; set; }
            public double Mediana { get; set; }
            public double Variance { get; set; }
        }
    }
}
