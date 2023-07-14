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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReservationRestaurantAdmin.Models;
using ReservationRestaurantAdmin.Models2;
using ReservationRestaurantAdmin.ModelsResponse;
using ReservationRestaurantAdmin.ModelsResponse.UserSytem;

namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationsController : Controller
    {
        private readonly BookingRestaurantContext _context;
        public INotyfService _notifyService { get; }
        public ReservationsController(BookingRestaurantContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        // GET: Admin/Reservations
        public async Task<IActionResult> Index()
        {
            try
            {
                string uri = "http://localhost:8080/api/Reservation";
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
                ResponseObject<List<Reservations>> reservation = JsonSerializer.Deserialize<ResponseObject<List<Reservations>>>(data, options);

                return View(reservation.data);
                //return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        // GET: Admin/Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            try
            {
                string uri = "http://localhost:8080/api/Reservation/detail?id=" + id;
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
                ResponseObject<Reservations> result = JsonSerializer.Deserialize<ResponseObject<Reservations>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: Admin/Reservations/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Admin/Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  /*      [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("startTime,endTime,date,number_guest,description,phone_cus,phone_guest")] Reservations reservation)
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
                    _notifyService.Success("Có lỗi xãy ra");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Success("Có lỗi xãy ra");
                return View(user);
            }
        }*/

        // GET: Admin/Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/Reservation/detail?id=" + id;
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
                ResponseObject<Reservations> result = JsonSerializer.Deserialize<ResponseObject<Reservations>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,startTime,endTime,date,number_guest,description,status,price,discount,feedback")] Reservations reservation)
        {

            try
            {
                if (!ModelState.IsValid)    //check valid data truyền về
                {
                    _notifyService.Success("Không đúng format data");
                    return View(reservation);
                }

                string uri = "http://localhost:8080/api/Reservation";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //parse obj ra json để gửi đi
                string data = JsonSerializer.Serialize(reservation);

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
                    return View(reservation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View(reservation);
            }
            /*  ViewData["Id"] = new SelectList(_context.Users, "Id", "Name", reservation.Id);
              return View(reservation);*/
        }   


        // GET: Admin/Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/Reservation/detail?id=" + id;
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
                ResponseObject<Reservations> result = JsonSerializer.Deserialize<ResponseObject<Reservations>>(data, options);

                if (result.data == null) return NotFound();

                return View(result.data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: Admin/Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                
                string uri = "http://localhost:8080/api/Reservation?id=" +id;
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Approved(int? id)
        {
            try
            {
                Reservations reservations = await getReservations(id);

                string uri = "http://localhost:8080/api/Reservation";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                reservations.status = true;
                //parse obj ra json để gửi đi
                
                string data = JsonSerializer.Serialize(reservations);

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
                    return View(reservations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View();
            }
        }


        public async Task<IActionResult> Reject(int? id)
        {
            try
            {
                Reservations reservations = await getReservations(id);

                string uri = "http://localhost:8080/api/Reservation";
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                reservations.status = false;
                //parse obj ra json để gửi đi

                string data = JsonSerializer.Serialize(reservations);

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
                    return View(reservations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _notifyService.Error("Có lỗi xãy ra");
                return View();
            }
        }
        public async Task<Reservations> getReservations(int? id)
        {
            try
            {
                string uri = "http://localhost:8080/api/Reservation/detail?id=" + id;
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
                ResponseObject<Reservations> result = JsonSerializer.Deserialize<ResponseObject<Reservations>>(data, options);

                if (result.data == null) return null;

                return result.data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
 