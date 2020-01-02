using Menu.Core.Models;

namespace Menu.Service
{
    public interface IFavoriteService
    {
        Favorite GetById(int id);

        void Create(Favorite favorite);

        void SaveChanges();
    }
}