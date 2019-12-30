using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class VenueService : IVenueService
    {
        private readonly MenuContext _context;

        public VenueService(MenuContext context)
        {
            _context = context;
        }

        public Venue GetDetailById(int id)
        {
            return _context.Venues
                           .Where(v => v.Id == id)
                           .FirstOrDefault();
        }

        public Venue GetById(int id)
        {
            return _context.Venues
                           .Where(v => v.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Venue venue)
        {
            _context.Venues.Add(venue);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}