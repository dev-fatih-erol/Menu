using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class KitchenService : IKitchenService
    {
        private readonly MenuContext _context;

        public KitchenService(MenuContext context)
        {
            _context = context;
        }

        public Kitchen GetByUsernameAndPassword(string username, string password)
        {
            return _context.Kitchens
                    .Where(k =>
                           k.Username == username &&
                           k.Password == password)
                    .FirstOrDefault();
        }

        public Kitchen GetById(int id)
        {
            return _context.Kitchens.Where(k => k.Id == id)
                                    .Select(k => new Kitchen
                                    {
                                         Id = k.Id,
                                         Venue = k.Venue
                                    }).FirstOrDefault();
        }

        public void Create(Kitchen kitchen)
        {
            _context.Kitchens.Add(kitchen);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}