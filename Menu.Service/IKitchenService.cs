using Menu.Core.Models;

namespace Menu.Service
{
    public interface IKitchenService
    {
        Kitchen GetById(int id);

        Kitchen GetByUsernameAndPassword(string username, string password);

        void Create(Kitchen kitchen);

        void SaveChanges();
    }
}