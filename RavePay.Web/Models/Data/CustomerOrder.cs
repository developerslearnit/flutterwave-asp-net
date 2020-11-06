using System;
using System.Collections.Generic;

namespace RavePay.Web.Models.data
{
    public partial class CustomerOrder
    {
        public int OrderId { get; set; }
        public string TransactionRef { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus { get; set; }
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
