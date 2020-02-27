using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderTableService
    {
        List<OrderTable> GetByReports(int venueId, bool isClosed);

        List<OrderTable> GetByOldTableId(int venueId, bool isClosed);

        OrderTable GetByGuest(int tableId);

        OrderTable GetDetailById(int id, int userId, bool isClosed);

        OrderTable GetDetailById(int id, int userId);

        OrderTable GetByUserId(int userId, bool isClosed);

        List<OrderTable> GetPendingByTableId(int tableId, bool isClosed);

        List<OrderTable> GetByTableId(int venueId, int tableId, bool isClosed);

        List<OrderTable> GetByUserId(int userId);

        OrderTable GetById(int id, int userId, bool isClosed);

        void Create(OrderTable orderTable);

        void SaveChanges();
    }
}