using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class PaymentByQiwi
    {

        public int Id { get; set; }

        public string NumberTo { get; set; }

        public string NumberFrom { get; set; }

        public string Status { get; set; }

    }
}