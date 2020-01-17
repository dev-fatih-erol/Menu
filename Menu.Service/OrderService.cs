using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderService : IOrderService
    {
        private readonly MenuContext _context;

        public OrderService(MenuContext context)
        {
            _context = context;
        }

        public List<Order> GetByUserId(int userId)
        {
            return _context.Orders
                           .Where(o => o.UserId == userId)
                           .Select(o => new Order
                           {
                              Id = o.Id,
                              OrderTable = o.OrderTable
                           }).ToList();
        }

        public void Create(Order order)
        {
            _context.Orders.Add(order);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}