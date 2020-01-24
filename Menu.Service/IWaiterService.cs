using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IWaiterService
    {
        List<Waiter> GetWithTableById(int id);

        Waiter GetByUsernameAndPassword(string username, string password);

        Waiter GetById(int id);

        void Create(Waiter waiter);

        void SaveChanges();
    }
}