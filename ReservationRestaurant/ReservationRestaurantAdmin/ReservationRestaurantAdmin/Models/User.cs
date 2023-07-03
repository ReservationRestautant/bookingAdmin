using System;
using System.Collections.Generic;

#nullable disable

namespace ReservationRestaurantAdmin.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public bool? Status { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
