using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderWaiterService : IOrderWaiterService
    {
        private readonly MenuContext _context;

        public OrderWaiterService(MenuContext context)
        {
            _context = context;
        }

        public OrderWaiter GetByOrderId(int orderId)
        {
            return _context.OrderWaiters
                           .Where(o => o.OrderId == orderId)
                           .FirstOrDefault();
        }

        public void Create(OrderWaiter orderWaiter)
        {
            _context.OrderWaiters.Add(orderWaiter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}