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

        public OrderTable GetByTableIdAndVenueId(int tableId, int venueId)
        {
            return _context.OrderTables
                           .Where(o => o.TableId == tableId &&
                                       o.VenueId == venueId)
                           .Select(o => new OrderTable
                           {
                               Id = o.Id,
                               IsClosed = o.IsClosed,
                               TableId = o.TableId,
                               VenueId = o.VenueId,
                               Order = o.Order
                           })
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