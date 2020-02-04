using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Pizzeriya.Models;
namespace Pizzeriya.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Buy()
        {
            double discount_user = 0;
            if (User.Identity.IsAuthenticated)
            {
                using (DBContext db = new DBContext())
                {
                    int Id = int.Parse(User.Identity.Name);
                    User user = db.Users.Where(u => u.UserId == Id).FirstOrDefault();
                    if (user != null)
                    {
                        discount_user = user.Discount;
                        ViewBag.User = user;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            else
            {
                ViewBag.User = new User();
            }
            var basket = (Basket)Session["Basket"];
            ViewBag.Actions = basket.GetDiscounts();
            ViewBag.Sum = basket.Sum;
            ViewBag.AllDiscounts = basket.GetDiscounts().Sum() + discount_user;
            int EndSum = (int)Math.Ceiling(basket.Sum * ((100 - basket.GetDiscounts().Sum() - discount_user) / 100.0));
            if (EndSum<500)
            {
                EndSum += 50;
            }
            ViewBag.SumWithDiscounts = EndSum;
            return View(basket.pizzaInBaskets);
        }

        [Authorize]
        public new ActionResult Profile()
        {
            using (DBContext db = new DBContext())
            {
                int Id = int.Parse(User.Identity.Name);
                User user = db.Users.Where(u => u.UserId == Id).FirstOrDefault();
                if (user != null)
                {
                    List<Order> orders = db.Orders.Where(o => o.UserId == Id).ToList();
                    Order last_order = orders.OrderBy(o => o.DateOfOrder).LastOrDefault();
                    if(last_order!=null)
                    {
                        ViewBag.Data = last_order.DateOfOrder;
                    }
                    else
                    {
                        ViewBag.Data = "еще не было";
                    }
                    ViewBag.UserName = user.FirstName;
                    return View(user);
                }
            }
            return HttpNotFound();
        }

        public JsonResult ChangeUser(string Name, string LastName, string Email, string Phone, string Streat, string House, string Padik, string Flat)
        {
            using (DBContext db = new DBContext())
            {
                int Id = int.Parse(User.Identity.Name);
                User user = db.Users.Where(u => u.UserId == Id).FirstOrDefault();
                if (user != null)
                {
                    user.FirstName = Name;
                    user.LastName = LastName;
                    user.Email = Email;
                    user.PhoneNumber = Phone;
                    user.Streat = Streat;
                    user.House = House;
                    user.Padik = Padik;
                    user.Flat = Flat;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("user_is_not_found", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Sign()
        {
            return View();
        }

        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("MainPage");
        }

        public ActionResult MainPage()
        {
            Session["CurAvaibl"] = "all";
            Session["CurSort"] = "name";
            var basket = new Basket() { Sum = 0 };
            Session["Basket"] = basket;
            List<string> ingradients = new List<string>();
            DBContext db = new DBContext();
            foreach (Pizza pizza in db.Pizzas.ToList())
            {
                string[] ingr = pizza.Composition.Split(',');
                foreach (string s in ingr)
                    if (!ingradients.Contains(s.Trim()))
                        ingradients.Add(s.Trim());
            }
            ViewBag.Ingr = ingradients;
            ViewBag.Basket = basket;

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
            sh = db.Shares.Where(s => s.DateBegin <= DateTime.Now && s.DateEnd >= DateTime.Now && (s.OnDay == "All" || s.OnDay == curDay)).ToList();
            ViewBag.Shares = sh;
            //if (db.Roles.Count() == 0)
            //{
            //    db.Roles.Add(new Role { Id = 1, Name = "user" });
            //    db.Roles.Add(new Role { Id = 2, Name = "admin" });
            //    db.Roles.Add(new Role { Id = 3, Name = "Operator" });
            //    db.Users.Add(new User
            //    {
            //        UserId = 1,
            //        Discount = 1,
            //        FirstName = "Admin",
            //        Sum = 0,
            //        Email = "admin@com",
            //        PhoneNumber = "+38admin",
            //        Password = "au74dy%114mu86iy%119nu87",
            //        RoleId = 2
            //    });
            //    db.Users.Add(new User
            //    {
            //        UserId = 2,
            //        Discount = 1,
            //        FirstName = "Operator",
            //        Sum = 0,
            //        Email = "oper@com",
            //        PhoneNumber = "+38oper",
            //        Password = "au74dy%114mu86iy%119nu87",
            //        RoleId = 3
            //    });
            //    db.SaveChanges();
            //}
            if (User.Identity.IsAuthenticated)
            {
                int id = int.Parse(User.Identity.Name);
                User user = db.Users.Where(u => u.UserId == id).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.UserName = user.FirstName;
                    ViewBag.Id = int.Parse(User.Identity.Name);
                    ViewBag.Role = user.RoleId;
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(db.Pizzas.OrderBy(p => p.PizzaAvaible).ToList());
        }

        public JsonResult RegisterUser(string Email, string Phone, string Pas)
        {
            using (DBContext db = new DBContext())
            {
                if (Email != string.Empty && db.Users.Where(u => u.Email == Email.Trim()).Count() != 0)
                {
                    return Json("user_error_email", JsonRequestBehavior.AllowGet);
                }
                if (Phone != string.Empty && db.Users.Where(u => u.PhoneNumber == Phone.Trim()).Count() != 0)
                {
                    return Json("user_error_phone", JsonRequestBehavior.AllowGet);
                }
                User LastUser = db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                int Id = 1;
                if (LastUser != null)
                {
                    Id = LastUser.UserId + 1;
                }
                db.Users.Add(new User
                {
                    UserId = Id,
                    FirstName="Unknown",
                    Sum = 0,
                    Discount = 1,
                    PhoneNumber = Phone != string.Empty ? Phone : "",
                    Email = Email != string.Empty ? Email : "",
                    Password = HashPass(Pas),
                    RoleId = 1
                });
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(Id.ToString(), true);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BasketAdd(PizzaInBasket pizza)
        {
            var basket = (Basket)Session["Basket"];
            foreach (var p in basket.pizzaInBaskets)
            {
                if (p.Equals(pizza))
                {
                    p.Count++;
                    basket.Sum += pizza.Cost;
                    Session["Basket"] = basket;
                    return Json(basket.Sum, JsonRequestBehavior.AllowGet);
                }
            }
            pizza.Count = 1;
            basket.pizzaInBaskets.Add(pizza);
            basket.Sum += pizza.Cost;
            Session["Basket"] = basket;
            return Json(basket.Sum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BasketRemove(PizzaInBasket pizza)
        {
            pizza.Count = 1;
            var basket = (Basket)Session["Basket"];
            foreach (var p in basket.pizzaInBaskets)
            {
                if (p.Equals(pizza))
                {
                    if (p.Count > 1)
                        p.Count--;
                    else
                        basket.pizzaInBaskets.Remove(pizza);
                    basket.Sum -= pizza.Cost;
                    Session["Basket"] = basket;
                    return Json(basket.Sum, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public static List<Pizza> GetBySort(string sort, List<Pizza> input)
        {
            switch (sort)
            {
                case "name":
                    return input.OrderBy(p => p.Name).ToList();
                case "cost":
                    return input.OrderBy(p => p.Cost).ToList();
                case "weight":
                    return input.OrderBy(p => p.Weight).ToList();
                case "action":
                    return input.OrderBy(p => p.Share).Reverse().ToList();
                case "popul":
                    DBContext db = new DBContext();
                    var pizzas = db.PIzzasInOrder.ToList();
                    int[] arr = new int[1000];
                    for (int i = 0; i < 1000; i++)
                        arr[i] = 0;
                    foreach (var p in pizzas)
                    {
                        arr[p.Id] += p.PizzaCount;
                    }
                    var ans = new List<Pizza>();
                    for (int i = 0; i < 1000; i++)
                    {
                        if (arr[i] != 0)
                        {
                            Pizza p = db.Pizzas.Where(piz => piz.PizzaId == i).FirstOrDefault();
                            if (p != null)
                            {
                                ans.Add(p);
                            }
                        }
                    }
                    foreach(var p in input)
                    {
                        bool contain = false;
                        foreach(var a in ans)
                        {
                            if(a.PizzaId==p.PizzaId)
                            {
                                contain = true;
                                break;
                            }
                        }
                        if(!contain)
                        {
                            ans.Add(p);
                        }
                    }
                    return ans;
                default:
                    return input;
            }
        }

        public static List<Pizza> GetByAvaible(string avaible, List<Pizza> input)
        {
            switch (avaible)
            {
                case "all":
                    return input;
                case "avaible":
                    return input.Where(p => p.PizzaAvaible == "Avaible").ToList();
                case "notavaible":
                    return input.Where(p => p.PizzaAvaible == "NotAvaible").ToList();
                default:
                    return input;
            }
        }

        public JsonResult GetPizza(string SortBy, string By)
        {
            DBContext db = new DBContext();
            var pizz = db.Pizzas.ToList();
            var CurSort = (string)Session["CurSort"];
            var CurAvaibl = (string)Session["CurAvaibl"];
            if (SortBy.Equals("sort"))
            {
                Session["CurSort"] = By;
                pizz = GetByAvaible(CurAvaibl, pizz);
                pizz = GetBySort(By, pizz);
                return Json(pizz, JsonRequestBehavior.AllowGet);
            }
            else if (SortBy.Equals("avaibl"))
            {
                Session["CurAvaibl"] = By;
                pizz = GetByAvaible(By, pizz);
                pizz = GetBySort(CurSort, pizz);
                return Json(pizz, JsonRequestBehavior.AllowGet);
            }
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPizzaIngradients(string notAllowedIngradients)
        {
            DBContext db = new DBContext();
            var pizz = db.Pizzas.ToList();
            var CurSort = (string)Session["CurSort"];
            var CurAvaibl = (string)Session["CurAvaibl"];
            if (notAllowedIngradients == "all")
            {
                pizz = GetByAvaible(CurAvaibl, pizz);
                pizz = GetBySort(CurSort, pizz);
                return Json(pizz, JsonRequestBehavior.AllowGet);
            }
            string[] notAllowed = notAllowedIngradients.Split(',');
            foreach (string ing in notAllowed)
            {
                if (pizz.Count == 0)
                    return Json("no pizza", JsonRequestBehavior.AllowGet);
                pizz = pizz.Where(p => !p.Composition.Contains(ing)).ToList();
            }
            pizz = GetByAvaible(CurAvaibl, pizz);
            pizz = GetBySort(CurSort, pizz);
            if (pizz.Count == 0)
            {
                return Json("no pizza", JsonRequestBehavior.AllowGet);
            }
            return Json(pizz, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult LoginUser(string Email_Phone, string Pas)
        {
            Pas = HashPass(Pas);
            using (DBContext db = new DBContext())
            {
                User User_Email = db.Users.Where(u => u.Email == Email_Phone && u.Password == Pas).FirstOrDefault();
                if (User_Email != null)
                {
                    FormsAuthentication.SetAuthCookie(User_Email.UserId.ToString(), true);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                User User_Phone = db.Users.Where(u => u.PhoneNumber == Email_Phone && u.Password == Pas).FirstOrDefault();
                if (User_Phone != null)
                {
                    FormsAuthentication.SetAuthCookie(User_Phone.UserId.ToString(), true);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("user_is_not_found", JsonRequestBehavior.AllowGet);
        }

        public static string HashPass(string pas)
        {
            string hash = "";
            for (int i = 0; i < pas.Length; i++)
            {
                hash += pas[i];
                if (i % 2 == 0)
                {
                    hash += "u";
                    hash += (pas[i] - 23);
                }
                else
                {
                    hash += "y%";
                    hash += (pas[i] + 14);
                }
            }
            return hash;
        }

        [Authorize(Roles="admin")]
        public ActionResult AdminMainPage()
        {
            DBContext db = new DBContext();
            int id = int.Parse(User.Identity.Name);
            User user = db.Users.Where(u => u.UserId == id).FirstOrDefault();
            if (user != null)
            {
                ViewBag.UserName = user.FirstName;
                ViewBag.Id = int.Parse(User.Identity.Name);
            }
            else
            {
                return HttpNotFound();
            }
            ViewBag.Actions = db.Shares.ToList();
            return View(db.Pizzas.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult RegPizza(bool IsNew, int Id = 0)
        {
            DBContext db = new DBContext();
            if (IsNew)
            {
                ViewBag.IsNew = true;
                var LastPizza = db.Pizzas.OrderByDescending(p => p.PizzaId).FirstOrDefault();
                if (LastPizza != null)
                {
                    ViewBag.NextId = LastPizza.PizzaId + 1;
                }
                else
                    ViewBag.NextId = 1;
                return View(new Pizza() { PizzaAvaible = "Avaible" });
            }
            ViewBag.IsNew = false;
            return View(db.Pizzas.Where(p => p.PizzaId == Id).FirstOrDefault());
        }

        [Authorize(Roles = "admin")]
        public ActionResult RegAction(bool IsNew, int Id = 0)
        {
            DBContext db = new DBContext();
            if (IsNew)
            {
                var LastShare = db.Shares.OrderByDescending(s => s.ShareId).FirstOrDefault();
                if (LastShare != null)
                {
                    ViewBag.NextId = LastShare.PizzaId + 1;
                }
                else
                    ViewBag.NextId = 1;
                ViewBag.Pizzas = db.Pizzas.ToList();
                return View(new Share() { OnDay = "All", PizzaId = 1 });
            }
            ViewBag.NextId = Id;
            ViewBag.Pizzas = db.Pizzas.ToList();
            return View(db.Shares.Where(s => s.ShareId == Id).FirstOrDefault());
        }

        public JsonResult RegPizzaResult(int Id, string Name, int Cost, int Weight, string Composition, string Avaible)
        {
            using (DBContext db = new DBContext())
            {
                var pizza = db.Pizzas.Where(p => p.PizzaId == Id).FirstOrDefault();
                if (pizza != null)
                {
                    pizza.PizzaId = Id;
                    pizza.Name = Name;
                    pizza.Cost = Cost;
                    pizza.Weight = Weight;
                    pizza.Composition = Composition;
                    pizza.PizzaAvaible = Avaible;
                    pizza.Share = "NotShare";
                    db.Entry(pizza).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Pizza NewPizza = new Pizza
                    {
                        PizzaId = Id,
                        Name = Name,
                        Cost = Cost,
                        Weight = Weight,
                        Composition = Composition,
                        Url = "src/img/pizza.png",
                        PizzaAvaible = Avaible,
                        Share = "NotShare",
                    };
                    db.Pizzas.Add(NewPizza);
                    db.SaveChanges();
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegActionResult(int Id, string Name, DateTime DateBegin, DateTime DateEnd, string OnDay,
            string Description, int Count, int PizzaId, int Discount)
        {
            using (DBContext db = new DBContext())
            {
                var action = db.Shares.Where(s => s.ShareId == Id).FirstOrDefault();
                if (action != null)
                {
                    action.ShareId = Id;
                    action.Name = Name;
                    action.DateBegin = DateBegin;
                    action.DateEnd = DateEnd;
                    action.OnDay = OnDay;
                    action.Description = Description;
                    action.PizzaCount = Count;
                    action.PizzaId = PizzaId;
                    action.Discount = Discount;
                    action.PizzaName = db.Pizzas.Where(p => p.PizzaId == PizzaId).FirstOrDefault().Name;
                    db.Entry(action).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Share NewShare = new Share
                    {
                        ShareId = Id,
                        Name = Name,
                        DateBegin = DateBegin,
                        DateEnd = DateEnd,
                        OnDay = OnDay,
                        Description = Description,
                        PizzaCount = Count,
                        PizzaId = PizzaId,
                        PizzaName = db.Pizzas.Where(p => p.PizzaId == PizzaId).FirstOrDefault().Name,
                        Discount = Discount
                    };
                    db.Shares.Add(NewShare);
                    db.SaveChanges();
                }
                var pizza = db.Pizzas.Where(p => p.PizzaId == PizzaId).FirstOrDefault();
                if (pizza != null)
                {
                    pizza.Share = "Share";
                    db.Entry(pizza).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles="operator")]
        public ActionResult OperatorMain()
        {
            DBContext db = new DBContext();
            List<Order> orders = db.Orders.ToList();
            orders.Reverse();
            orders = orders.Take(10).ToList();
            orders.Reverse();
            for (int i = 0; i < orders.Count; i++)
            {
                int id = orders[i].OrderId;
                var PizzaInOrder = db.PIzzasInOrder.Where(p => p.OrderId == id).ToList();
                orders[i].pizzaInOrder = PizzaInOrder;
            }
            return View(orders);
        }

        public JsonResult GetOrder(string Count, string By)
        {
            DBContext db = new DBContext();
            var orders = db.Orders.ToList();
            switch (By)
            {
                case "date":
                    break;
                case "view":                   
                    orders = orders.Where(o => o.View=="View").ToList();
                    break;
                case "ready":
                    orders = orders.Where(o => o.Ready == "Ready").ToList();
                    break;
                default:
                    break;
            }
            if (Count != "all")
            {
                int cnt = int.Parse(Count);
                orders.Reverse();
                orders = orders.Take(cnt).ToList();
                orders.Reverse();
            }   
            for (int i = 0; i < orders.Count; i++)
            {
                int id = orders[i].OrderId;
                var PizzaInOrder = db.PIzzasInOrder.Where(p => p.OrderId == id).ToList();
                orders[i].pizzaInOrder = PizzaInOrder;
            }
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateOrder(string FirstName,string LastName,string Email,string PhoneNumber,
            string Streat,string House,string Padik,string Flat,string Payment,string Add
                )
        {
            DBContext db = new DBContext();
            Order last = db.Orders.OrderByDescending(o => o.OrderId).FirstOrDefault();
            int id = 1;
            if (last != null)
                id = last.OrderId + 1;
            int payId = 1;
            switch(Payment)
            {
                case "qiwi":
                    Payment = "PaymentByQIWI";
                    var lastPayQIWI = db.PaymentsByQIWI.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (lastPayQIWI != null)
                        payId = lastPayQIWI.Id + 1;
                    break;
                case "cash":
                    Payment = "PaymentByCash";
                    var lastPayCash = db.PaymentsByCash.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (lastPayCash != null)
                        payId = lastPayCash.Id + 1;
                    break;
                case "contactless":
                    Payment = "PaymentByContactless";
                    var lastPayCont = db.PaymentsByContactless.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (lastPayCont != null)
                        payId = lastPayCont.Id + 1;
                    break;
                case "card":
                    Payment = "PaymentByCard";
                    var lastPayCard = db.PaymentsByCard.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (lastPayCard != null)
                        payId = lastPayCard.Id + 1;
                    break;
            }
            User user = null;
            if(User.Identity.IsAuthenticated)
            {
                int id_u = int.Parse(User.Identity.Name);
                user = db.Users.Where(u => u.UserId == id_u).FirstOrDefault();
            }
            int? idUser = null;
            double discount_user = 0;
            var basket = (Basket)Session["Basket"];
            if (user!=null)
            {
                idUser = user.UserId;
                discount_user = user.Discount;
                user.Sum += (float)Math.Ceiling(basket.Sum * ((100 - basket.GetDiscounts().Sum() - discount_user) / 100.0));
                user.Discount *= (float)1.1;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            }          
            Order order = new Order
            {
                OrderId = id,
                UserId = idUser,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Streat = Streat,
                House = House,
                Padik = Padik,
                Flat = Flat,

                Cost = Math.Ceiling(basket.Sum * ((100 - basket.GetDiscounts().Sum() - discount_user) / 100.0)),
                PaymentMethod = Payment,
                PaymentId = payId,
                Additional = Add,
                DateOfOrder = DateTime.Now.ToString(),
                View = "NotView",
                Ready = "NotReady"
            };
            db.Orders.Add(order);
            db.SaveChanges();
            var lastPizza = db.PIzzasInOrder.OrderByDescending(p => p.Id).FirstOrDefault();
            int idPizza = 1;
            if (lastPizza != null)
                idPizza = lastPizza.Id + 1;
            for(int i=0;i<basket.pizzaInBaskets.Count;i++)
            {
                PizzaInOrder pizz = new PizzaInOrder
                {
                    Id = idPizza,
                    PizzaId = basket.pizzaInBaskets[i].Id,
                    PizzaCount = basket.pizzaInBaskets[i].Count,
                    Name = basket.pizzaInBaskets[i].Name,
                    OrderId = id,
                    PizzaSize = basket.pizzaInBaskets[i].Size,
                    PizzaWeight = basket.pizzaInBaskets[i].Weight
                };
                db.PIzzasInOrder.Add(pizz);
                db.SaveChanges();
                idPizza++;
            }

            var shares = basket.GetSharesId();
            var lastShare = db.SharesInOrder.OrderByDescending(s => s.Id).FirstOrDefault();
            int idShare = 1;
            if (lastShare!= null)
                idShare = lastShare.Id + 1;
            foreach (int idS in shares)
            {
                ShareInOrder sh = new ShareInOrder
                {
                    Id = idShare,
                    OrderId = id
                };
                db.SharesInOrder.Add(sh);
                db.SaveChanges();
                idShare++;
            }
            basket.pizzaInBaskets.Clear();
            basket.Sum = 0;
            Session["Basket"] = basket;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetOrderView(int Id, bool IsView)
        {
            using (DBContext db = new DBContext())
            {
                Order order = db.Orders.Where(o => o.OrderId == Id).FirstOrDefault();
                if(order!=null)
                {
                    if(IsView)
                    {
                        order.View = "View";
                    }
                    else
                    {
                        order.View = "NotView";
                    }
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetOrderReady(int Id, bool IsReady)
        {
            using (DBContext db = new DBContext())
            {
                Order order = db.Orders.Where(o => o.OrderId == Id).FirstOrDefault();
                if (order != null)
                {
                    if (IsReady)
                    {
                        order.Ready = "Ready";
                    }
                    else
                    {
                        order.Ready = "NotReady";
                    }
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Theme(string theme)
        {
            Response.Cookies["theme"].Value = theme;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Done()
        {
            return View();
        }

        [Authorize(Roles="admin")]
        public ActionResult DeletePizza(int Id)
        {
            using (DBContext db = new DBContext())
            {
                var Pizza = db.Pizzas.Where(p => p.PizzaId == Id).FirstOrDefault();
                if (Pizza != null)
                {
                    db.Entry(Pizza).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("AdminMainPage");
                }
                else
                    return HttpNotFound();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteAction(int Id)
        {
            using (DBContext db = new DBContext())
            {
                var Action = db.Shares.Where(s => s.ShareId == Id).FirstOrDefault();
                if (Action != null)
                {
                    db.Entry(Action).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("AdminMainPage");
                }
                else
                    return HttpNotFound();
            }
        }
    }
}