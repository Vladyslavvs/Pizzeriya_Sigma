using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class Share
    {

        public int ShareId { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public string OnDay { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PizzaId { get; set; }

        public int PizzaCount { get; set; }

        public string PizzaName { get; set; }

        public int Discount { get; set; }

    }
}