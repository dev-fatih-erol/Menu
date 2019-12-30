using System;
using System.Collections.Generic;
using System.Linq;
using Menu.Core.Enums;
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

        public List<Venue> GetRandomByVenueType(VenueType venueType, int count)
        {
            return _context.Venues
                           .Where(v => v.VenueType == venueType)
                           .OrderBy(v => Guid.NewGuid())
                           .Take(count)
                           .ToList();
        }

        public List<Venue> GetRandom(int count)
        {
            return _context.Venues
                           .OrderBy(v => Guid.NewGuid())
                           .Take(count)
                           .ToList();
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