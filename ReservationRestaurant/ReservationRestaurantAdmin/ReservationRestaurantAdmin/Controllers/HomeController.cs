using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationRestaurantAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login(string Username, string Password)
        {
            if(Username == "admin" && Password == "1")
            {
                return Redirect("/Admin");
            }   
            return View();
        }
    }
}
