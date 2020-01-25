using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderTableService
    {
        OrderTable GetDetailById(int id, int userId, bool isClosed);

        OrderTable GetDetailById(int id, int userId);

        OrderTable GetByUserId(int userId, bool isClosed);

        List<OrderTable> GetPendingByTableId(int tableId, bool isClosed);

        List<OrderTable> GetByTableId(int tableId, bool isClosed);

        List<OrderTable> GetByUserId(int userId);

        OrderTable GetById(int id, int userId, bool isClosed);

        void Create(OrderTable orderTable);

        void SaveChanges();
    }
}