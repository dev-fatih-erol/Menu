using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class UserService : IUserService
    {
        private readonly MenuContext _context;

        public UserService(MenuContext context)
        {
            _context = context;
        }

        public User GetByIdAndPassword(int id, string password)
        {
            return _context.Users
                           .Where(u =>
                                  u.Id == id &&
                                  u.Password == password)
                           .FirstOrDefault();
        }

        public User GetByPhoneNumberAndPassword(string phoneNumber, string password)
        {
            return _context.Users
                           .Where(u =>
                                  u.PhoneNumber == phoneNumber &&
                                  u.Password == password)
                           .FirstOrDefault();
        }

        public User GetByPhoneNumber(string phoneNumber)
        {
            return _context.Users
                           .Where(u => u.PhoneNumber == phoneNumber)
                           .FirstOrDefault();
        }

        public User GetById(int id)
        {
            return _context.Users
                           .Where(u => u.Id == id)
                           .FirstOrDefault();
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}