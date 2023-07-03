using Microsoft.AspNetCore.Mvc;

namespace ReservationRestaurantAdmin.Areas.Areas.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
