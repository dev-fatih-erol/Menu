using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderCashService
    {
        OrderCash GetByUserIdAndOrderTableId(int userId, int orderTableId);

        void Create(OrderCash orderCash);

        void SaveChanges();
    }
}