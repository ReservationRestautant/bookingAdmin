using System;
using System.Collections.Generic;

#nullable disable

namespace ReservationRestaurantAdmin.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            BookingTables = new HashSet<BookingTable>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? NumGuest { get; set; }
        public string PhoneGuest { get; set; }
        public double? Price { get; set; }
        public int? Discount { get; set; }
        public bool? Status { get; set; }
        public string Feedback { get; set; }
        public int? UserId { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<BookingTable> BookingTables { get; set; }
    }
}
