using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using ReservationRestaurantAdmin.ModelsResponse.TableSystem;

namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleTablesController : Controller
    {
        private readonly BookingRestaurantContext _context;
        public INotyfService _notifyService { get; }
        public ScheduleTablesController(BookingRestaurantContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/ScheduleTables
        public async Task<IActionResult> Index()
        {
            try
            {
                string uri = "http://localhost:8080/api/ScheduleTable";
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
                ResponseObject<List<ScheduleTables>> table = JsonSerializer.Deserialize<ResponseObject<List<ScheduleTables>>>(data, options);

                return View(table.data);
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/ScheduleTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/ScheduleTable/detail?id=" + id;
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
                ResponseObject<ScheduleTables> result = JsonSerializer.Deserialize<ResponseObject<ScheduleTables>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/ScheduleTables/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Admin/ScheduleTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("startTime,endTime,date,table_id")] ScheduleTables scheduleTable)
        {
            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(scheduleTable);
                }

                string uri = "http://localhost:8080/api/ScheduleTable";
                using HttpClient client = new HttpClient();


                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(scheduleTable);

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
                    _notifyService.Error("Có lỗi xãy ra");
                    return View(scheduleTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(scheduleTable);
            }
        }

        // GET: Admin/ScheduleTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/ScheduleTable/detail?id=" + id;
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
                ResponseObject<ScheduleTables> result = JsonSerializer.Deserialize<ResponseObject<ScheduleTables>>(data, options);
                ScheduleTables schedule = await getScheduleById(id);
                if (result.data == null) return NotFound();
                ViewData["table_id"] = new SelectList(await getListTable(), "id", "name", schedule.tableRestautant.id);
                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/ScheduleTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("startTime,endTime,date,")] ScheduleTables schedule)
        {
            try
            {
                ScheduleTables scheduleTables = await getScheduleById(id);

                string uri = "http://localhost:8080/api/ScheduleTable";
                using HttpClient client = new HttpClient();


                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                scheduleTables.startTime = schedule.startTime;
                scheduleTables.endTime = schedule.endTime;
                scheduleTables.date = schedule.date;
                scheduleTables.table_id = schedule.tableRestautant.id;
                //parse obj ra json để gửi đi

                string data = JsonSerializer.Serialize(scheduleTables);

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
                    return View(scheduleTables);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View();
            }
        }





        // GET: Admin/ScheduleTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/ScheduleTable/detail?id=" + id;
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
                ResponseObject<ScheduleTables> result = JsonSerializer.Deserialize<ResponseObject<ScheduleTables>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ScheduleTables> getScheduleById(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/ScheduleTable/detail?id=" + id;
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
                ResponseObject<ScheduleTables> result = JsonSerializer.Deserialize<ResponseObject<ScheduleTables>>(data, options);

                if (result.data == null) return null;

                return result.data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/ScheduleTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ScheduleTables scheduleTable = await getScheduleById(id);
                string uri = "http://localhost:8080/api/ScheduleTable?id=" + id;
                using HttpClient client = new HttpClient();


                //add jwt vào header
                string token = HttpContext.Session.GetString("TOKEN");
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(scheduleTable);

                //config vào content dể gửi đi
                var contentdata = new StringContent(data, System.Text.Encoding.UTF8, "application/json");	//nhớ viết đầy đủ này nha, thiếu UTF8 và "application/json" thì nó sẽ xuất error 415 (Unsupported Media Type).

                //call api wiith content data ở trên
                HttpResponseMessage response = await client.DeleteAsync(uri);			//update nên gọi Put

                response.EnsureSuccessStatusCode(); //check call

                if (response.IsSuccessStatusCode)
                {
                    //call api success
                    _notifyService.Success("Xóa thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //error, can't call api
                    _notifyService.Error("Có lỗi xãy ra");
                    return View(scheduleTable);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool ScheduleTableExists(int id)
        {
            return _context.ScheduleTables.Any(e => e.Id == id);
        }

        public async Task<List<TableRestaurant>> getListTable()
        {
            try
            {
                string uri = "http://localhost:8080/api/TableRestautant/all";
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
                ResponseListTable table = JsonSerializer.Deserialize<ResponseListTable>(data, options);

                return  table.data.ToList();
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
