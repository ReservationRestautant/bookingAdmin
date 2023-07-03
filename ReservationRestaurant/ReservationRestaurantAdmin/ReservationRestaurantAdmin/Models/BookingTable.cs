using System;
using System.Collections.Generic;

#nullable disable

namespace ReservationRestaurantAdmin.Models
{
    public partial class BookingTable
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Table Table { get; set; }
    }
}
