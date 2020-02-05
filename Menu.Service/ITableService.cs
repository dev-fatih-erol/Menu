using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ITableService
    {
        List<Table> GetByVenueId(int venueId);

        Table GetById(int id, int venueId);

        Table GetById(int id);

        void Create(Table table);

        void SaveChanges();
    }
}