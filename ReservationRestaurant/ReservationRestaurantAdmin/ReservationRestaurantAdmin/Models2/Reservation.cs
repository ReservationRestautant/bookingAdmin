using System;

namespace ReservationRestaurantAdmin.Models2
{
    public class Reservation
    {
        public int id { get; set; }

        public DateTime date_create { get; set; }
  
        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public int number_guest { get; set; }

        public string phone_guest { get; set; }

        public string description { get; set; }

        public float price { get; set; }

        public int discount { get; set; }

        public string feedback { get; set; }
      
        public bool status { get; set; }

    }
}
