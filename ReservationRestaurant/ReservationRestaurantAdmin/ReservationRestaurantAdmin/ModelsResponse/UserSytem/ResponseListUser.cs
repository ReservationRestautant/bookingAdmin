using ReservationRestaurantAdmin.Models2;
using System.Collections.Generic;

namespace ReservationRestaurantAdmin.ModelsResponse.UserSytem
{
    public class ResponseListUser
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<UserSystem> data { get; set; }
    }
}
