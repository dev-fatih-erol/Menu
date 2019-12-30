using System.Linq;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenueService
    {
        Venue GetDetailById(int id);

        Venue GetById(int id);

        void Create(Venue venue);

        void SaveChanges();
    }
}