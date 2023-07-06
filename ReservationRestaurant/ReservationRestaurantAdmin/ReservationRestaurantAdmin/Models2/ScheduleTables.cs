using System;
using System.Threading;
using System.Xml.Linq;


namespace ReservationRestaurantAdmin.Models2
{
    public class ScheduleTables
    {
        public int id { get; set; }

        public DateTime date { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public int table_id { get; set; }
        public TableRestaurant tableRestautant { get; set; }

        /*  public override string ToString()
          {
              return $"id: {id} - date: {date} - startTime: {startTime} - endTime: {endTime}  - tableId: {tableId}";
          }*/
    }
}
