using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavePay.Web.Models
{
    public class CartModel
    {
        public string customerName { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }

        public string amount { get; set; }

    }
}
