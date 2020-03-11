using System.Collections.Generic;
using System.Linq;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OrderService : IOrderService
    {
        private readonly MenuContext _context;

        public OrderService(MenuContext context)
        {
            _context = context;
        }

        public Order GetByTestId(int id)
        {
            return _context.Orders
                           .Where(o => o.Id == id).Select(o => new Order
                           {
                               OrderTable = new OrderTable
                               {
                                   IsClosed = o.OrderTable.IsClosed,
                               }
                           })
                           .FirstOrDefault();
        }

        public List<Order> GetByVenueId(int venueId, OrderStatus orderStatus)
        {
            return _context.Orders
                           .Where(o => o.OrderTable.Venue.Id == venueId &&
                                       o.OrderStatus == orderStatus)
                           .OrderBy(o => o.CreatedDate)
                           .Select(o => new Order {
                               Id = o.Id,
                               Code = o.Code,
                               Description = o.Description,
                               OrderStatus = o.OrderStatus,
                               CreatedDate = o.CreatedDate,
                               OrderDetail = o.OrderDetail,
                               OrderTableId = o.OrderTableId,
                               OrderTable = new OrderTable
                               {
                                   Id = o.Id,
                                   IsClosed = o.OrderTable.IsClosed,
                                   CreatedDate = o.OrderTable.CreatedDate,
                                   VenueId = o.OrderTable.VenueId,
                                   TableId = o.OrderTable.TableId,
                                   Table = o.OrderTable.Table,
                                   UserId = o.OrderTable.UserId,
                                   User = o.OrderTable.User
                               },
                               OrderWaiter = new OrderWaiter
                               {
                                   Id = o.OrderWaiter.Id,
                                   CreatedDate = o.OrderWaiter.CreatedDate,
                                   OrderId = o.OrderWaiter.OrderId,
                                   WaiterId = o.OrderWaiter.WaiterId,
                                   Waiter = new Waiter
                                   {
                                       Id = o.OrderWaiter.Waiter.Id,
                                       Name = o.OrderWaiter.Waiter.Name,
                                       Surname = o.OrderWaiter.Waiter.Surname,
                                       Username = o.OrderWaiter.Waiter.Username,
                                       Password = o.OrderWaiter.Waiter.Password,
                                       CreatedDate = o.OrderWaiter.Waiter.CreatedDate
                                   }
                               }
                           }).ToList();
        }

        public Order GetById(int id, OrderStatus orderStatus)
        {
            return _context.Orders
                           .Where(o => o.Id == id &&
                                       o.OrderStatus == orderStatus)
                           .FirstOrDefault();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                           .Where(o => o.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Order order)
        {
            _context.Orders.Add(order);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}