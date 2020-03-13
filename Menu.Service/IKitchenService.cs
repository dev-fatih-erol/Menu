using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IKitchenService
    {
        Kitchen GetByIds(int id);

        List<Kitchen> GetByVenueId(int venueId);

        Kitchen GetByUsernameAndPassword(string username, string password);

        Kitchen GetById(int id);

        void Create(Kitchen kitchen);

        void SaveChanges();
    }
}