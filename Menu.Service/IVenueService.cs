﻿using System.Collections.Generic;
using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenueService
    {
        List<Venue> GetByCriteria(string name);

        List<Venue> GetRandom(VenueType? venueType, int take);

        Venue GetDetailById(int id);

        Venue GetById(int id);

        void Create(Venue venue);

        void SaveChanges();
    }
}