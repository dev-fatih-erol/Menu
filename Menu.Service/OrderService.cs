using System.Linq;
using Menu.Core.Enums;
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

        public Order GetById(int id, OrderStatus orderStatus)
        {
            return _context.Orders
                           .Where(o => o.Id == id &&
                                       o.OrderStatus == orderStatus)
                           .FirstOrDefault();
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