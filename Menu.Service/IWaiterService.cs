﻿using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IWaiterService
    {
        Waiter GetByUsernameAndPassword(string username, string password);

        List<Waiter> GetByVenueId(int venueId);

        Waiter GetById(int id);

        void Create(Waiter waiter);

        void SaveChanges();
    }
}