using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICashService
    {
        Cash GetByIds(int id);

        List<Cash> GetByVenueId(int venueId);

        Cash GetByUsernameAndPassword(string username, string password);

        Cash GetById(int id);

        void Create(Cash cash);

        void SaveChanges();
    }
}