using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using ReservationRestaurantAdmin.ModelsResponse.TableSystem;
using ReservationRestaurantAdmin.ModelsResponse.UserSytem;

namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TablesController : Controller
    {
        private readonly BookingRestaurantContext _context;
        public INotyfService _notifyService { get; }
        public TablesController(BookingRestaurantContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/Tables
        public async Task<IActionResult> Index()
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/all";
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
                ResponseListTable table = JsonSerializer.Deserialize<ResponseListTable>(data, options);

                return View(table.data);
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/Tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/detail?id=" + id;
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
                ResponseObject<TableRestaurant> result = JsonSerializer.Deserialize<ResponseObject<TableRestaurant>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/Tables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name")] TableRestaurant table)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(table);
                }

                string uri = "http://localhost:8080/api/TableRestautant";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(table);

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
                    _notifyService.Success("Có lỗi xãy ra");
                    return View(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Success("Có lỗi xãy ra");
                return View(table);
            }

        }

        // GET: Admin/Tables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/detail?id=" + id;
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
                ResponseObject<TableRestaurant> result = JsonSerializer.Deserialize<ResponseObject<TableRestaurant>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/Tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,capacity")] TableRestaurant table)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(table);
                }

                string uri = "http://localhost:8080/api/TableRestautant";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(table);

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
                    _notifyService.Success("Có lỗi xãy ra");
                    return View(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Success("Có lỗi xãy ra");
                return View(table);
            }
        }

        // GET: Admin/Tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/detail?id=" + id;
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
                ResponseObject<TableRestaurant> result = JsonSerializer.Deserialize<ResponseObject<TableRestaurant>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<TableRestaurant> getId(int id)
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/detail?id=" + id;
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
                ResponseObject<TableRestaurant> result = JsonSerializer.Deserialize<ResponseObject<TableRestaurant>>(data, options);

                if (result.data == null) return null;

                return result.data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        // POST: Admin/Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TableRestaurant table = await getId(id);
                string uri = "http://localhost:8080/api/TableRestautant?id=" + id;
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(table);

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
                    return View(table);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.Id == id);
        }
    }
}
