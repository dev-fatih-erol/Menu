﻿using System.Collections.Generic;
using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderService
    {
        public List<Order> GetByVenueId(int venueId);

        Order GetById(int id, OrderStatus orderStatus);

        void Create(Order order);

        void SaveChanges();
    }
}