using System;
using System.Collections.Generic;

#nullable disable

namespace ReservationRestaurantAdmin.Models
{
    public partial class Table
    {
        public Table()
        {
            BookingTables = new HashSet<BookingTable>();
            ScheduleTables = new HashSet<ScheduleTable>();
        }

        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookingTable> BookingTables { get; set; }
        public virtual ICollection<ScheduleTable> ScheduleTables { get; set; }
    }
}
