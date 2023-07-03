using ReservationRestaurantAdmin.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoHttpClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ResponeUser responeUser = await getListUser();
            foreach(var item in responeUser.data)
            {
                Console.WriteLine(item);
            }

        }
        public static async Task<ResponeUser> getListUser()
        {
            try
            {
                string uri = "http://localhost:8080/api/Customer";
                using HttpClient client = new HttpClient();

                //add header
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //thực thi gọi GET tới uri
                var response = await client.GetAsync(uri);

                //Phát sinh Exception nếu truy vấn có mã trả về không thành công
                response.EnsureSuccessStatusCode();

             /*   //in header ra để xem
                ShowHeaders(response.Headers);*/

                //lấy data về thành chuỗi string json
                string data = await response.Content.ReadAsStringAsync();

                //parse string thành json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ResponeUser user = JsonSerializer.Deserialize<ResponeUser>(data, options);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }
    }
}
