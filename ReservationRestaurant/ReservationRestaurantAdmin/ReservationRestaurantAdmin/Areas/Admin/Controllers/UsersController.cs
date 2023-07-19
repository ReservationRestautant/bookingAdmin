using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using ReservationRestaurantAdmin.ModelsResponse.UserSytem;

namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly BookingRestaurantContext _context;
        public INotyfService _notifyService { get; }

        public UsersController(BookingRestaurantContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer";

                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponseListUser user = JsonSerializer.Deserialize<ResponseListUser>(data, options);

                return View(user.data);
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer/search?phone=" + phone;
                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponseObject<UserSystem> result = JsonSerializer.Deserialize<ResponseObject<UserSystem>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,password,phone")] UserSystem user)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(user);
                }

                string uri = "http://localhost:8080/api/Customer";
                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(user);

                //config vào content dể gửi đi
                var contentdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");	//nhớ viết đầy đủ này nha, thiếu UTF8 và "application/json" thì nó sẽ xuất error 415 (Unsupported Media Type).

                //call api wiith content data ở trên
                HttpResponseMessage response = await client.PostAsync(uri, contentdata);			

                response.EnsureSuccessStatusCode(); //check call

                if (response.IsSuccessStatusCode)
                {
                    //call api success
                    _notifyService.Success("Tạo thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //error, can't call api
                    _notifyService.Warning("Có lỗi xãy ra");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(user);
            }

        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer/search?phone=" + phone;
                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponseObject<UserSystem> result = JsonSerializer.Deserialize<ResponseObject<UserSystem>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,password,phone,role,status")] UserSystem user)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(user);
                }

                string uri = "http://localhost:8080/api/Customer?action=Update";
                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(user);

                //config vào content dể gửi đi
                var contentdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");	//nhớ viết đầy đủ này nha, thiếu UTF8 và "application/json" thì nó sẽ xuất error 415 (Unsupported Media Type).

                //call api wiith content data ở trên
                HttpResponseMessage response = await client.PutAsync(uri, contentdata);			//update nên gọi Put

                response.EnsureSuccessStatusCode(); //check call

                if (response.IsSuccessStatusCode)
                {
                    //call api success
                    _notifyService.Success("Cập nhật thành công"); 
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //error, can't call api
                    _notifyService.Warning("Có lỗi xãy ra");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(user);
            }

        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer/search?phone=" + phone;
                using HttpClient client = new HttpClient();

                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;


                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponseObject<UserSystem> result = JsonSerializer.Deserialize<ResponseObject<UserSystem>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return View(user);
        }

        public async Task<UserSystem> getByPhone(string phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer/search?phone=" + phone;
                using HttpClient client = new HttpClient();


                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponseObject<UserSystem> result = JsonSerializer.Deserialize<ResponseObject<UserSystem>>(data, options);

                if (result.data == null) return null;

                return result.data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string phone)
        {
            try
            {
                UserSystem user = await getByPhone(phone);

                string uri = "http://localhost:8080/api/Customer?action=DeActive";
                using HttpClient client = new HttpClient();


                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(user);

                //config vào content dể gửi đi
                var contentdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");	//nhớ viết đầy đủ này nha, thiếu UTF8 và "application/json" thì nó sẽ xuất error 415 (Unsupported Media Type).

                //call api wiith content data ở trên
                HttpResponseMessage response = await client.PutAsync(uri, contentdata);			//update nên gọi Put

                response.EnsureSuccessStatusCode(); //check call

                if (response.IsSuccessStatusCode)
                {
                    //call api success
                    _notifyService.Success("De active thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //error, can't call api
                    _notifyService.Success("Có lỗi xãy ra");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //var user = await _context.Users.FindAsync(id);
            //_context.Users.Remove(user);
            //await _context.SaveChangesAsync();
            //_notifyService.Success("Xóa thành công");
            //return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
