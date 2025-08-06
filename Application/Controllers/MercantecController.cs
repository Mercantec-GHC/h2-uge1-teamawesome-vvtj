using Microsoft.AspNetCore.Mvc;
using BlazorWASM.Services;

namespace Application.Controllers
{
    public class MercantecController : Controller
    {
        private readonly APIService _apiService; 

        public MercantecController(APIService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("api/benzin")]
        public async Task<IActionResult> GetBenzin()
        {
            var data = await _apiService.GetBenzinAsync();
            return Json(data);
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
