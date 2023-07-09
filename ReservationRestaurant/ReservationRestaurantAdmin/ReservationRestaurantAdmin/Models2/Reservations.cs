using System;

namespace ReservationRestaurantAdmin.Models2
{
    public class Reservations
    {
        public int id { get; set; }

        public string date { get; set; }
        public string date_create { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public int number_guest { get; set; }

        public string phone_guest { get; set; }

        public string description { get; set; }

        public float price { get; set; }

        public int discount { get; set; }

        public string feedback { get; set; }
      
        public bool status { get; set; }
        
        public UserSystem userSysterm { get; set; }

        

    }
}
