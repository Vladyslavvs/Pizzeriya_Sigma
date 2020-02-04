using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class PaymentByCard
    {

        public int Id { get; set; }

        public string CardTo { get; set; }

        public string CardFrom { get; set; }

        public string Status { get; set; }

    }
}