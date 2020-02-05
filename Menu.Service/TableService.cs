using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class TableService : ITableService
    {
        private readonly MenuContext _context;

        public TableService(MenuContext context)
        {
            _context = context;
        }

        public List<Table> GetByVenueId(int venueId)
        {
            return _context.Tables
                           .Where(t => t.Venue.Id == venueId)
                           .Select(t => new Table
                           {
                               Id = t.Id,
                               Name = t.Name,
                               TableStatus = t.TableStatus,
                               CreatedDate = t.CreatedDate,
                               VenueId = t.VenueId
                           }).ToList();
        }

        public Table GetById(int id, int venueId)
        {
            return _context.Tables
                           .Where(t => t.Id == id &&
                                       t.VenueId == venueId)
                           .FirstOrDefault();
        }

        public Table GetById(int id)
        {
            return _context.Tables
                           .Where(t => t.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Table table)
        {
            _context.Tables.Add(table);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}