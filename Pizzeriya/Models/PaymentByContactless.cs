using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class PaymentByContactless
    {

        public int Id { get; set; }

        public string PaymentSystem { get; set; }

        public string Status { get; set; }

    }
}