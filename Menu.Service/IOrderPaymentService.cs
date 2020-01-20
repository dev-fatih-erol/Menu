using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderPaymentService
    {
        void Create(OrderPayment orderPayment);

        void SaveChanges();
    }
}