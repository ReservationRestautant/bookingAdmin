using System.ComponentModel.DataAnnotations;

namespace ReservationRestaurantAdmin.Models2
{
    public class Account
    {
        [Required(ErrorMessage ="Phone is required")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
