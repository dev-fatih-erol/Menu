using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderPaymentService : IOrderPaymentService
    {
        private readonly MenuContext _context;

        public OrderPaymentService(MenuContext context)
        {
            _context = context;
        }

        public void Create(OrderPayment orderPayment)
        {
            _context.OrderPayments.Add(orderPayment);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}