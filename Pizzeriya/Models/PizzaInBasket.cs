using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class PizzaInBasket
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public int Weight { get; set; }

        public int Size { get; set; }

        public int Count { get; set; }

        public override bool Equals(object another)
        {
            PizzaInBasket two = (PizzaInBasket)another;
            return this.Name == two.Name && this.Cost == two.Cost &&
                this.Weight == two.Weight && this.Size == two.Size;
        }

    }
}