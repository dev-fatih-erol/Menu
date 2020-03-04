using Menu.Core.Models;

namespace Menu.Service
{
    public interface IManagerService
    {
        Manager GetByUsernameAndPassword(string username, string password);

        void Create(Manager manager);

        void SaveChanges();
    }
}