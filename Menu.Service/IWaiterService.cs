using Menu.Core.Models;

namespace Menu.Service
{
    public interface IWaiterService
    {
        Waiter GetById(int id);

        void Create(Waiter waiter);

        void SaveChanges();
    }
}