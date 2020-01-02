using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly MenuContext _context;

        public FavoriteService(MenuContext context)
        {
            _context = context;
        }

        public Favorite GetById(int id)
        {
            return _context.Favorites
                           .Where(f => f.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}