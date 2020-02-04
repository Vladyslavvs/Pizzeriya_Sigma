using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeriya.Models
{
    public class ShareInOrder
    {

        public int Id { get; set; }

        public int ShareId { get; set; }

        public int OrderId { get; set; }

    }
}