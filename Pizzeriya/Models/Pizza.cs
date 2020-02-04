using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class Pizza
    {

        public int PizzaId { get; set; }

        public string Name { get; set; }

        public float Cost { get; set; }

        public int Weight { get; set; }

        public string Composition { get; set; }

        public string Url { get; set; }

        public string PizzaAvaible { get; set; }

        public string Share { get; set; }
    }
}