using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHttpClient.Model
{
    public class Customer
    {
        public int id {  get; set; }

        public string name { get; set; }

        public string password { get; set; }

        public string phone { get; set; }

        public char role { get; set; }

        public bool status { get; set; }

        public override string ToString()
        {
            return $"id: {id} - name: {name} - pwd: {password} - phone: {phone} - role: {role} - status {status}";
        }
    }
}
