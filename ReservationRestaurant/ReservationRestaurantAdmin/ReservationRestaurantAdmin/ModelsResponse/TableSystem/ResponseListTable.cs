using ReservationRestaurantAdmin.Models2;
using System.Collections.Generic;

namespace ReservationRestaurantAdmin.ModelsResponse.TableSystem
{
    public class ResponseListTable
    {

        public string status { get; set; }

        public string message { get; set; }

        public List<TableRestaurant> data { get; set; }
    }
}
