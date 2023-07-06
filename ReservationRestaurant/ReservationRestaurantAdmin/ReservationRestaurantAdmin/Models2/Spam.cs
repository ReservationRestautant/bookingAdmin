using System;

namespace ReservationRestaurantAdmin.Models2
{
    public class Spam
    {
        public string phone { get; set; }

        public DateTime spamDay { get; set; }
        public DateTime spamWeek { get; set; }

        public bool block { get; set; }

        public string timeUnblock { get; set; }
    }
}
