using System;

namespace ReservationRestaurantAdmin.Models2
{
    public class SpamVM
    {
        public string phone { get; set; }

        public int spamDay { get; set; }
        public int spamWeek { get; set; }

        public bool block { get; set; }

        public string timeUnblock { get; set; }
    }
}
