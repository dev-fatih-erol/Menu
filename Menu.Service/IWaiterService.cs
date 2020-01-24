using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IWaiterService
    {
        List<Waiter> GetWithTableById(int id);

        Waiter GetById(int id);

        void Create(Waiter waiter);

        void SaveChanges();
    }
}