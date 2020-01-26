using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderPaymentService
    {
        List<OrderPayment> GetByIsClosedAndTableId(int tableId, bool isClosed);

        void Create(OrderPayment orderPayment);

        void SaveChanges();
    }
}