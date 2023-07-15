using System;
using System.ComponentModel.DataAnnotations;

namespace ReservationRestaurantAdmin.Models2
{
    public class SpamVM
    {
        [Phone(ErrorMessage = "Must a phone number")]
        public string phone { get; set; }

        public int spamDay { get; set; }
        public int spamWeek { get; set; }

        public bool block { get; set; }

        public string timeUnblock { get; set; }
    }
}
