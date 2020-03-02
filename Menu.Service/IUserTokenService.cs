using Menu.Core.Models;

namespace Menu.Service
{
    public interface IUserTokenService
    {
        UserToken GetByUserId(int userId);

        void Create(UserToken userToken);

        void SaveChanges();
    }
}