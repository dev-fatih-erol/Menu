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