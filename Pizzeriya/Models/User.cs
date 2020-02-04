using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class User
    {

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Streat { get; set; }

        public string House { get; set; }

        public string Padik { get; set; }

        public string Flat { get; set; }

        public string PhoneNumber { get; set; }

        public float Discount { get; set; }

        public float Sum { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

    }
}