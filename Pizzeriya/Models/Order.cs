using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class Order
    {

        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Streat { get; set; }

        public string House { get; set; }

        public string Padik { get; set; }

        public string Flat { get; set; }

        public double Cost { get; set; }

        public string PaymentMethod { get; set; }

        public int PaymentId { get; set; }

        public string Additional { get; set; }

        public string DateOfOrder { get; set; }

        public ICollection<ShareInOrder> SharesInOrder { get; set; }

        public ICollection<PizzaInOrder> pizzaInOrder { get; set; }

        public Order()
        {
            SharesInOrder= new List<ShareInOrder>();
            pizzaInOrder = new List<PizzaInOrder>();
        }

        public string View { get; set; }

        public string Ready { get; set; }
    }
}