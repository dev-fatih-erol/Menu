using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenueService
    {
        Venue GetById(int id);

        void Create(Venue venue);

        void SaveChanges();
    }
}