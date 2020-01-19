using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderService
    {
        void Create(Order order);

        void SaveChanges();
    }
}