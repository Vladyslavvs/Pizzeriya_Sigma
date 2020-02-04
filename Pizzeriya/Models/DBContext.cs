using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Pizzeriya.Models
{
    public class DBContext:DbContext
    {

        public DBContext() : base("ConnectionString")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Share> Shares { get; set; }

        public DbSet<PizzaInOrder> PIzzasInOrder { get; set; }

        public DbSet<ShareInOrder> SharesInOrder { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<PaymentByCash> PaymentsByCash { get; set; }

        public DbSet<PaymentByCard> PaymentsByCard { get; set; }

        public DbSet<PaymentByContactless> PaymentsByContactless { get; set; }

        public DbSet<PaymentByQiwi> PaymentsByQIWI { get; set; }

    }

}