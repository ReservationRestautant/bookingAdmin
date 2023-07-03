using System;
using System.Collections.Generic;

#nullable disable

namespace ReservationRestaurantAdmin.Models
{
    public partial class ScheduleTable
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TableId { get; set; }

        public virtual Table Table { get; set; }
    }
}
