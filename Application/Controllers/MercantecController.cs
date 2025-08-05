using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class MercantecController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
