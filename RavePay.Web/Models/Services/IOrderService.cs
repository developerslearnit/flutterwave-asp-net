using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavePay.Web.Models.Services
{
    public interface IOrderService
    {
        Task<string> AddOrder(string trxRef,Guid customerId,decimal amount);

        Task<Guid> AddCustomer(CartModel model);

        Task<bool> UpdateOrder(string orderRef, bool status);
    }
}
