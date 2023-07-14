using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.ModelsResponse.UserSytem;
using System.Net.Http;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using ReservationRestaurantAdmin.ModelsResponse.Spam;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using System.Net.Http.Headers;
using System.Net.Http;


namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpamController : Controller
    {
        public INotyfService _notifyService { get; }
        public SpamController( INotyfService notifyService)
        {
            _notifyService = notifyService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string uri = "http://localhost:8080/api/spam";
                using HttpClient client = new HttpClient();

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
                ResponseListSpam user = JsonSerializer.Deserialize<ResponseListSpam>(data, options);

                return View(user.data);
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> Details(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/spam/detail?phone=" + phone;
                using HttpClient client = new HttpClient();

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
                ResponseObject<SpamVM> result = JsonSerializer.Deserialize<ResponseObject<SpamVM>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public IActionResult Create()
        {
            return View();
        }
        //"phone,spamDay,spamWeek,block,timeUnBlock"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string phone,[Bind("phone")] SpamVM spam)
        {
            try
            {
                spam.spamDay = 0;
                spam.spamWeek = 0;
                spam.block = false;
                spam.timeUnblock = null;
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(spam);
                }

                string uri = "http://localhost:8080/api/spam";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(spam);

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
                    return View(spam);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(spam);
            }

        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/spam/detail?phone=" + phone;
                using HttpClient client = new HttpClient();

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
                ResponseObject<SpamVM> result = JsonSerializer.Deserialize<ResponseObject<SpamVM>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string phone, [Bind("phone,spamDay,spamWeek,block,timeUnBlock")] SpamVM spam)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(spam);
                }

                string uri = "http://localhost:8080/api/spam";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(spam);

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
                    return View(spam);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(spam);
            }
        }

        public async Task<IActionResult> Delete(string? phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/spam/detail?phone=" + phone;
                using HttpClient client = new HttpClient();

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
                ResponseObject<SpamVM> result = JsonSerializer.Deserialize<ResponseObject<SpamVM>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SpamVM> getId(string phone)
        {
            try
            {
                string uri = "http://localhost:8080/api/spam/detail?phone=" + phone;
                using HttpClient client = new HttpClient();

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
                ResponseObject<SpamVM> result1 = JsonSerializer.Deserialize<ResponseObject<SpamVM>>(data, options);

                if (result1.data == null) return null;

                return result1.data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string phone)
        {
            try
            {
                SpamVM spamVM = await getId(phone);
                string uri = "http://localhost:8080/api/spam?phone=" + phone;
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(spamVM);

                //config vào content dể gửi đi
                var contentdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");	//nhớ viết đầy đủ này nha, thiếu UTF8 và "application/json" thì nó sẽ xuất error 415 (Unsupported Media Type).

                //call api wiith content data ở trên
                HttpResponseMessage response = await client.DeleteAsync(uri);			//update nên gọi Put

                response.EnsureSuccessStatusCode(); //check call

                if (response.IsSuccessStatusCode)
                {
                    //call api success
                    _notifyService.Success("Delete thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //error, can't call api
                    _notifyService.Error("Có lỗi xãy ra");
                    return View(spamVM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
