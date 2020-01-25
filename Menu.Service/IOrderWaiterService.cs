using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderWaiterService
    {
        OrderWaiter GetByOrderId(int orderId);

        void Create(OrderWaiter orderWaiter);

        void SaveChanges();
    }
}