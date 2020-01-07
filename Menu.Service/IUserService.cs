using Menu.Core.Models;

namespace Menu.Service
{
    public interface IUserService
    {
        User GetByIdAndPassword(int id, string password);

        User GetByPhoneNumberAndPassword(string phoneNumber, string password);

        User GetByPhoneNumber(string phoneNumber);

        User GetById(int id);

        void Create(User user);

        void SaveChanges();
    }
}