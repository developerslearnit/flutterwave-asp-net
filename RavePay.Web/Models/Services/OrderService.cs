using Microsoft.EntityFrameworkCore;
using RavePay.Web.Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RavePay.Web.Models.Services
{
    public class OrderService : IOrderService
    {

        private readonly RaveDbContext _context;
        public OrderService(RaveDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddCustomer(CartModel model)
        {
            var customer = new Customer()
            {
                Address = model.address,
                CustomenrName =model.customerName,
                DateRegistered = DateTime.Now,
                Email =model.email,
                Telephone =model.phone
            };

            _context.Customer.Add(customer);

            var cust = await _context.SaveChangesAsync();

            return customer.CustomerId;

        }

        public async Task<string> AddOrder(string trxRef, Guid customerId, decimal amount)
        {
            var order = new CustomerOrder()
            {
                Amount =amount,
                CustomerId = customerId,
                OrderDate =DateTime.Now,
                OrderStatus =false,
                TransactionRef =trxRef,
               
            };

            _context.CustomerOrder.Add(order);

            await _context.SaveChangesAsync();

            return order.TransactionRef;
        }

        public async Task<bool> UpdateOrder(string orderRef,bool status)
        {
            var order = await _context.CustomerOrder.FirstOrDefaultAsync(od => od.TransactionRef == orderRef);
            if (order != null)
            {
                order.OrderStatus = status;               
            }

            await _context.SaveChangesAsync();

            return true;
        }

      
    }
}
