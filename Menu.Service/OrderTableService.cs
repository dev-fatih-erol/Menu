using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderTableService : IOrderTableService
    {
        private readonly MenuContext _context;

        public OrderTableService(MenuContext context)
        {
            _context = context;
        }

        public OrderTable GetByTableIdAndVenueId(int tableId, int venueId, bool isClosed)
        {
            return _context.OrderTables
                           .Where(o => o.TableId == tableId &&
                                       o.VenueId == venueId &&
                                       o.IsClosed == isClosed)
                           .FirstOrDefault();
        }

        public OrderTable GetByTableIdAndVenueId(int tableId, int venueId, int userId, bool isClosed)
        {
            return _context.OrderTables
                           .Where(o => o.TableId == tableId &&
                                       o.VenueId == venueId &&
                                       o.Order.Any(o => o.UserId == userId))
                           .FirstOrDefault();
        }

        public void Create(OrderTable orderTable)
        {
            _context.OrderTables.Add(orderTable);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}