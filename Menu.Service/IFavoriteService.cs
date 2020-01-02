using Menu.Core.Models;

namespace Menu.Service
{
    public interface IFavoriteService
    {
        Favorite GetByUserIdAndVenueId(int userId, int venueId);

        Favorite GetById(int id);

        void Create(Favorite favorite);

        void SaveChanges();
    }
}