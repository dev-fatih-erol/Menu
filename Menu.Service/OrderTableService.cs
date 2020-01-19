using System.Collections.Generic;
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

        public OrderTable GetByUserId1(int userId)
        {
            return _context.OrderTables
                           .Where(o => o.UserId == userId)
                           .Select(o => new OrderTable
                           {
                               Id = o.Id,
                               IsClosed = o.IsClosed,
                               CreatedDate = o.CreatedDate,
                               VenueId = o.VenueId,
                            
                               TableId = o.TableId,
                           
                               UserId = o.UserId,
                               Order = o.Order.Select(p => new Order
                               {
                                   Code = p.Code,
                                   CreatedDate = p.CreatedDate,
                                  
                         
                               }).ToList(),
                               OrderPayment = o.OrderPayment
                           }).FirstOrDefault();
        }

        public OrderTable GetByUserId(int userId, bool isClosed)
        {
            return _context.OrderTables
                           .Where(o => o.UserId == userId &&
                                       o.IsClosed == isClosed)
                           .Select(o => new OrderTable
                           {
                               Id = o.Id,
                               IsClosed = o.IsClosed,
                               CreatedDate = o.CreatedDate,
                               VenueId = o.VenueId,
                               TableId = o.TableId,
                               UserId = o.UserId
                           })
                           .FirstOrDefault();
        }

        public List<OrderTable> GetByUserId(int userId)
        {
            return _context.OrderTables
                           .Where(o => o.UserId == userId)
                           .Select(o => new OrderTable
                           {
                               Id = o.Id,
                               IsClosed = o.IsClosed,
                               CreatedDate = o.CreatedDate,
                               VenueId = o.VenueId,
                               Venue = o.Venue,
                               TableId = o.TableId,
                               UserId = o.UserId,
                               Order = o.Order
                           }).ToList();
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