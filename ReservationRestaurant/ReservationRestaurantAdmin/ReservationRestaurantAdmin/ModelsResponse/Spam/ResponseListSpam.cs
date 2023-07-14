using ReservationRestaurantAdmin.Models2;
using System.Collections.Generic;

namespace ReservationRestaurantAdmin.ModelsResponse.Spam
{
    public class ResponseListSpam
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<SpamVM> data { get; set; }
    }
}
