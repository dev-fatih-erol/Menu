using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class UserTokenService : IUserTokenService
    {
        private readonly MenuContext _context;

        public UserTokenService(MenuContext context)
        {
            _context = context;
        }

        public UserToken GetByUserId(int userId)
        {
            return _context.UserTokens
                           .Where(o => o.UserId == userId)
                           .FirstOrDefault();
        }

        public void Create(UserToken userToken)
        {
            _context.UserTokens.Add(userToken);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}