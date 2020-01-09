using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly MenuContext _context;

        public OrderDetailService(MenuContext context)
        {
            _context = context;
        }

        public void Create(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}