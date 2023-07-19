using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using ReservationRestaurantAdmin.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationRestaurantAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public INotyfService _notifyService { get; }

        public HomeController(ILogger<HomeController> logger, INotyfService notifyService)
        {
            _notifyService = notifyService;
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

        public async Task<IActionResult> Login(Account account )
        {
            try
            {
                string uri = " http://localhost:8080/api/auth/login?phone="+ account.phone+"&password=" + account.password;
                using HttpClient client = new HttpClient();

                     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     HttpResponseMessage response = await client.PostAsync(uri, null);
                     response.EnsureSuccessStatusCode();
                     var responseString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    ResponseObject<DataLogin> result = JsonSerializer.Deserialize<ResponseObject<DataLogin>>(responseString, options);
                    HttpContext.Session.SetString("TOKEN" , result.data.token);

                if (response.IsSuccessStatusCode)
                {
                    //call api success
               
                    _notifyService.Success("Login Admin Success");
                    return Redirect("/Admin");
                }
                else
                {
                    //error, can't call api
                
                    return RedirectToAction("Index");
            
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }
    }
}
