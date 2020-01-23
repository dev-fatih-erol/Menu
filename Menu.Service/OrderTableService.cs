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

        public OrderTable GetDetailById(int id, int userId, bool isClosed)
        {
            return _context.OrderTables.Where(o => o.Id == id &&
                                                   o.UserId == userId &&
                                                   o.IsClosed == isClosed)
                                       .Select(o => new OrderTable
                                       {
                                           Id = o.Id,
                                           IsClosed = o.IsClosed,
                                           CreatedDate = o.CreatedDate,
                                           VenueId = o.VenueId,
                                           Venue = o.Venue,
                                           TableId = o.TableId,
                                           Table = o.Table,
                                           UserId = o.UserId,
                                           Order = o.Order.Select(o => new Order
                                           {
                                               Id = o.Id,
                                               Code = o.Code,
                                               Description = o.Description,
                                               OrderStatus = o.OrderStatus,
                                               CreatedDate = o.CreatedDate,
                                               OrderTableId = o.OrderTableId,
                                               OrderDetail = o.OrderDetail
                                           }).ToList()
                                       }).FirstOrDefault();
        }

        public OrderTable GetDetailById(int id, int userId)
        {
            return _context.OrderTables.Where(o => o.Id == id &&
                                                   o.UserId == userId &&
                                                   o.IsClosed == true)
                                       .Select(o => new OrderTable
                                       {
                                           Id = o.Id,
                                           IsClosed = o.IsClosed,
                                           CreatedDate = o.CreatedDate,
                                           VenueId = o.VenueId,
                                           Venue = o.Venue,
                                           TableId = o.TableId,
                                           Table = o.Table,
                                           UserId = o.UserId,
                                           Order = o.Order.Select(o => new Order
                                           {
                                               Id = o.Id,
                                               Code = o.Code,
                                               Description = o.Description,
                                               OrderStatus = o.OrderStatus,
                                               CreatedDate = o.CreatedDate,
                                               OrderTableId = o.OrderTableId,
                                               OrderDetail = o.OrderDetail
                                           }).ToList(),
                                           OrderPayment = new OrderPayment
                                           {
                                               Id = o.OrderPayment.Id,
                                               Tip = o.OrderPayment.Tip,
                                               EarnedPoint = o.OrderPayment.EarnedPoint,
                                               UsedPoint = o.OrderPayment.UsedPoint,
                                               CreatedDate = o.OrderPayment.CreatedDate,
                                               OrderTableId = o.OrderPayment.OrderTableId,
                                               VenuePaymentMethodId = o.OrderPayment.VenuePaymentMethodId,
                                               VenuePaymentMethod = new VenuePaymentMethod
                                               {
                                                   Id = o.OrderPayment.VenuePaymentMethod.Id,
                                                   CreatedDate = o.OrderPayment.VenuePaymentMethod.CreatedDate,
                                                   VenueId = o.OrderPayment.VenuePaymentMethod.VenueId,
                                                   PaymentMethodId = o.OrderPayment.VenuePaymentMethod.PaymentMethodId,
                                                   PaymentMethod = o.OrderPayment.VenuePaymentMethod.PaymentMethod
                                               }
                                           }
                                       }).FirstOrDefault();
        }

        public OrderTable GetByUserId(int userId, bool isClosed)
        {
            return _context.OrderTables.Where(o => o.UserId == userId &&
                                                   o.IsClosed == isClosed)
                                       .Select(o => new OrderTable
                                       {
                                           Id = o.Id,
                                           IsClosed = o.IsClosed,
                                           CreatedDate = o.CreatedDate,
                                           VenueId = o.VenueId,
                                           TableId = o.TableId,
                                           UserId = o.UserId,
                                           Order = o.Order.Select(o => new Order
                                           {
                                               Id = o.Id,
                                               Code = o.Code,
                                               Description = o.Description,
                                               OrderStatus = o.OrderStatus,
                                               CreatedDate = o.CreatedDate,
                                               OrderTableId = o.OrderTableId,
                                               OrderDetail = o.OrderDetail
                                           }).ToList(),
                                           OrderPayment = o.OrderPayment
                                       }).FirstOrDefault();
        }

        public List<OrderTable> GetByUserId(int userId)
        {
            return _context.OrderTables
                           .Where(o => o.UserId == userId)
                           .OrderByDescending(o => o.CreatedDate)
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