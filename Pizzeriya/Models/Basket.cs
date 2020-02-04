using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class Basket
    {

        public List<PizzaInBasket> pizzaInBaskets { get; set; }

        public double Sum { get; set; }

        public Basket()
        {
            pizzaInBaskets = new List<PizzaInBasket>();
        }

        public List<int> GetSharesId ()
        {
            List<int> list = new List<int>();
            bool[] VisitedShares = new bool[1000];
            for (int i = 0; i < VisitedShares.Length; i++)
                VisitedShares[i] = false;
            List<Share> sh = new List<Share>();
            string curDay = "non";
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    curDay = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    curDay = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    curDay = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    curDay = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    curDay = "Friday";
                    break;
                case DayOfWeek.Sunday:
                    curDay = "Sunday";
                    break;
                case DayOfWeek.Saturday:
                    curDay = "Saturday";
                    break;
                default:
                    curDay = "non";
                    break;
            }
            DBContext db = new DBContext();
            sh = db.Shares.Where(s => s.DateBegin <= DateTime.Now && s.DateEnd >= DateTime.Now && (s.OnDay == "All" || s.OnDay == curDay)).ToList();
            foreach (var pizza in pizzaInBaskets)
            {
                if (!VisitedShares[pizza.Id])
                {
                    VisitedShares[pizza.Id] = true;
                    var Action = sh.Where(s => s.PizzaId == pizza.Id).FirstOrDefault();
                    if (Action != null)
                    {
                        int Count = pizzaInBaskets.Where(p => p.Id == pizza.Id).Sum(p => p.Count);
                        if (Count >= Action.PizzaCount)
                        {
                            list.Add(Action.ShareId);
                        }
                    }
                }
            }
            return list;
        }

        public List<int> GetDiscounts()
        {
            List<int> list = new List<int>();
            bool[] VisitedShares = new bool[1000];
            for (int i = 0; i < VisitedShares.Length; i++)
                VisitedShares[i] = false;
            List<Share> sh = new List<Share>();
            string curDay = "non";
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    curDay = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    curDay = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    curDay = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    curDay = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    curDay = "Friday";
                    break;
                case DayOfWeek.Sunday:
                    curDay = "Sunday";
                    break;
                case DayOfWeek.Saturday:
                    curDay = "Saturday";
                    break;
                default:
                    curDay = "non";
                    break;
            }
            DBContext db = new DBContext();
            sh = db.Shares.Where(s => s.DateBegin <= DateTime.Now && s.DateEnd >= DateTime.Now && (s.OnDay == "All" || s.OnDay == curDay)).ToList();
            foreach (var pizza in pizzaInBaskets)
            {
                if (!VisitedShares[pizza.Id])
                {
                    VisitedShares[pizza.Id] = true;
                    var Action = sh.Where(s => s.PizzaId == pizza.Id).FirstOrDefault();
                    if (Action != null)
                    {
                        int Count = pizzaInBaskets.Where(p => p.Id == pizza.Id).Sum(p => p.Count);
                        if (Count >= Action.PizzaCount)
                        {
                            list.Add(Action.Discount);
                        }
                    }
                }
            }
            return list;
        }
    }
}