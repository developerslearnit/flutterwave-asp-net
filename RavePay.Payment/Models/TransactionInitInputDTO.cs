using System;
using System.Collections.Generic;
using System.Text;

namespace RavePay.Payment.Models
{
   
    public class TransactionInitInputDTO
    {
        public string tx_ref { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string redirect_url { get; set; }
        public string payment_options { get; set; }
        public Meta meta { get; set; }
        public Customer customer { get; set; }
        public Customizations customizations { get; set; }
    }

    public class Meta
    {
        public int consumer_id { get; set; }
        public string consumer_mac { get; set; }
    }

    public class Customer
    {
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string name { get; set; }
    }

    public class Customizations
    {
        public string title { get; set; }
        public string description { get; set; }
        public string logo { get; set; }
    }
}
