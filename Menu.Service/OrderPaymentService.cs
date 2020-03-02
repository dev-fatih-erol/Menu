using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderPaymentService : IOrderPaymentService
    {
        private readonly MenuContext _context;

        public OrderPaymentService(MenuContext context)
        {
            _context = context;
        }

        public List<OrderPayment> GetByIsClosedAndTableId(int tableId, bool isClosed)
        {
            return _context.OrderPayments.Where(o => o.OrderTable.TableId == tableId &&
                                                     o.OrderTable.IsClosed == isClosed)
                                       .Select(o => new OrderPayment
                                       {
                                           Id = o.Id,
                                           Tip = o.Tip,
                                           EarnedPoint = o.EarnedPoint,
                                           UsedPoint = o.UsedPoint,
                                           CreatedDate = o.CreatedDate,
                                           OrderTableId = o.OrderTableId,
                                           OrderTable = new OrderTable {
                                               Id = o.OrderTable.Id,
                                               IsClosed = o.OrderTable.IsClosed,
                                               CreatedDate = o.OrderTable.CreatedDate,
                                               VenueId = o.OrderTable.VenueId,
                                               TableId = o.OrderTable.TableId,
                                               UserId = o.OrderTable.UserId,
                                               User = o.OrderTable.User,
                                               Order = o.OrderTable.Order.Select(o => new Order
                                               {
                                                   Id = o.Id,
                                                   Code = o.Code,
                                                   Description = o.Description,
                                                   OrderStatus = o.OrderStatus,
                                                   CreatedDate = o.CreatedDate,
                                                   OrderTableId = o.OrderTableId,
                                                   OrderDetail = o.OrderDetail
                                               }).ToList()
                                           },
                                           VenuePaymentMethodId = o.VenuePaymentMethodId,
                                           VenuePaymentMethod = new VenuePaymentMethod
                                           {
                                               Id = o.VenuePaymentMethod.Id,
                                               CreatedDate = o.VenuePaymentMethod.CreatedDate,
                                               VenueId = o.VenuePaymentMethod.VenueId,
                                               PaymentMethodId = o.VenuePaymentMethod.PaymentMethodId,
                                               PaymentMethod = o.VenuePaymentMethod.PaymentMethod
                                           }
                                       }).ToList();
        }

        public OrderPayment GetByOrderTableId(int orderTableId)
        {
            return _context.OrderPayments
                           .Where(o => o.OrderTableId == orderTableId)
                           .Select(x => new OrderPayment
                           {
                               VenuePaymentMethod = new VenuePaymentMethod
                               {
                                   PaymentMethod = x.VenuePaymentMethod.PaymentMethod
                               }
                           }).FirstOrDefault();
        }

        public OrderPayment GetById(int id)
        {
            return _context.OrderPayments
                           .Where(o => o.Id == id).FirstOrDefault();
        }

        public void Create(OrderPayment orderPayment)
        {
            _context.OrderPayments.Add(orderPayment);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}