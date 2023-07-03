using DemoHttpClient.Model;
using ReservationRestaurantAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHttpClient
{
    public class ResponeUser
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<Customer> data { get; set; }
    }
}
