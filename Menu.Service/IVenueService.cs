using System.Collections.Generic;
using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenueService
    {
        List<Venue> GetRandomByVenueType(VenueType venueType, int count);

        List<Venue> GetRandom(int count);

        Venue GetDetailById(int id);

        Venue GetById(int id);

        void Create(Venue venue);

        void SaveChanges();
    }
}