using Menu.Core.Models;

namespace Menu.Service
{
    public interface IKitchenService
    {
        Kitchen GetByUsernameAndPassword(string username, string password);

        void Create(Kitchen kitchen);

        void SaveChanges();
    }
}