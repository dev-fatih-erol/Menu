using System.Collections.Generic;
using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderService
    {
        Order GetByTestId(int id);

        List<Order> GetByVenueId(int venueId, OrderStatus orderStatus);

        Order GetById(int id, OrderStatus orderStatus);

        Order GetById(int id);

        void Create(Order order);

        void SaveChanges();
    }
}