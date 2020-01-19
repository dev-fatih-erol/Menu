using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderTableService
    {
        OrderTable GetByUserId1(int userId);

        OrderTable GetByUserId(int userId, bool isClosed);

        List<OrderTable> GetByUserId(int userId);

        void Create(OrderTable orderTable);

        void SaveChanges();
    }
}