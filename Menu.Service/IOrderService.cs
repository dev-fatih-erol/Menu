using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderService
    {
        Order GetById(int id, OrderStatus orderStatus);

        void Create(Order order);

        void SaveChanges();
    }
}