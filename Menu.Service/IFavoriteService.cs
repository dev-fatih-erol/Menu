using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IFavoriteService
    {
        Favorite GetByUserIdAndVenueId(int userId, int venueId);

        List<Favorite> GetByUserId(int userId);

        Favorite GetById(int id);

        void Delete(Favorite favorite);

        void Create(Favorite favorite);

        void SaveChanges();
    }
}