using Menu.Core.Models;

namespace Menu.Service
{
    public interface IUserService
    {
        User GetByPhoneNumber(string phoneNumber);

        User GetById(int id);

        void Create(User user);

        void SaveChanges();
    }
}