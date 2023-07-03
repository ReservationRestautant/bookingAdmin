using ReservationRestaurantAdmin.Models2;
using System.Collections.Generic;

namespace ReservationRestaurantAdmin.ModelsResponse
{
    public class ResponseObject<T>
    {
        public string status { get; set; }

        public string message { get; set; }

        public T data { get; set; }
    }
}
