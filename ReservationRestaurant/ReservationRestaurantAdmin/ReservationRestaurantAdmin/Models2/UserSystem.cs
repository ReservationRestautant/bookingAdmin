namespace ReservationRestaurantAdmin.Models2
{
    public class UserSystem
    {
        public int id { get; set; }

        public string name { get; set; }

        public string password { get; set; }

        public string phone { get; set; }

        public string role { get; set; }

        public bool status { get; set; }

        public override string ToString()
        {
            return $"id: {id} - name: {name} - pwd: {password} - phone: {phone} - role: {role} - status {status}";
        }
    }
}
