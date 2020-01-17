using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderService
    {
        List<Order> GetByUserId(int userId);

        void Create(Order order);

        void SaveChanges();
    }
}