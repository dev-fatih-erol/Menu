using System;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderCashService : IOrderCashService
    {
        private readonly MenuContext _context;

        public OrderCashService(MenuContext context)
        {
            _context = context;
        }

        public OrderCash GetByUserIdAndOrderTableId(int userId, int orderTableId)
        {
            return _context.OrderCashes
                           .Where(o => o.OrderTable.User.Id == userId &&
                                       o.OrderTable.Id == orderTableId)
                           .FirstOrDefault();
        }

        public void Create(OrderCash orderCash)
        {
            _context.OrderCashes.Add(orderCash);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
