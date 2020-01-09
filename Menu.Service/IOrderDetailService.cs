using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderDetailService
    {
        void Create(OrderDetail orderDetail);

        void SaveChanges();
    }
}