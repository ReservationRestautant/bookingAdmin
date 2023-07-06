using System.Data;

namespace ReservationRestaurantAdmin.Models2
{
    public class TableRestaurant
    {
        public int id {  get; set; }

        public string name { get; set; }    

        public int capacity { get; set; }

        public override string ToString()
        {
            return $"id: {id} - name: {name} - capacity: {capacity}";
        }
    }
}
