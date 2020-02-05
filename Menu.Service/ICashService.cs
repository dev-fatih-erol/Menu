using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICashService
    {
        Cash GetByUsernameAndPassword(string username, string password);

        Cash GetById(int id);

        void Create(Cash cash);

        void SaveChanges();
    }
}