using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class PizzaInOrder
    {

        public int Id { get; set; }

        public int PizzaId { get; set; }

        public int PizzaCount { get; set; }

        public int PizzaSize { get; set; }

        public int PizzaWeight { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

    }
}