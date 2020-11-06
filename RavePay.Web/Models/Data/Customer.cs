using System;
using System.Collections.Generic;

namespace RavePay.Web.Models.data
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public Guid CustomerId { get; set; }
        public string CustomenrName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public DateTime? DateRegistered { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
