using System;
using System.Collections.Generic;
using System.Linq;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Data;
using Menu.Service.Extensions;

namespace Menu.Service
{
    public class VenueService : IVenueService
    {
        private readonly MenuContext _context;

        public VenueService(MenuContext context)
        {
            _context = context;
        }

        public Venue GetPaymentMethodById(int id)
        {
            return _context.Venues
                            .Where(v => v.Id == id)
                            .Select(v => new Venue
                            {
                                Id = v.Id,
                                Name = v.Name,
                                Photo = v.Photo,
                                Address = v.Address,
                                OpeningTime = v.OpeningTime,
                                ClosingTime = v.ClosingTime,
                                Latitude = v.Latitude,
                                Longitude = v.Longitude,
                                VenueType = v.VenueType,
                                CreatedDate = v.CreatedDate,
                                VenuePaymentMethod = v.VenuePaymentMethod.Select(v => new VenuePaymentMethod
                                {
                                    Id = v.Id,
                                    CreatedDate = v.CreatedDate,
                                    VenueId = v.VenueId,
                                    PaymentMethodId = v.PaymentMethodId,
                                    PaymentMethod = v.PaymentMethod
                                }).ToList()
                            }).FirstOrDefault();
        }

        public List<Venue> GetByCriteria(string name)
        {
            return _context.Venues
                           .WhereIf(name != null, v => v.Name.Contains(name))
                           .ToList();
        }

        public List<Venue> GetRandom(VenueType? venueType, int take)
        {
            return _context.Venues
                           .WhereIf(venueType != null, v => v.VenueType == venueType)
                           .OrderBy(v => Guid.NewGuid())
                           .Take(take)
                           .Select(v => new Venue
                           {
                               Id = v.Id,
                               Name = v.Name,
                               Photo = v.Photo,
                               Address = v.Address,
                               OpeningTime = v.OpeningTime,
                               ClosingTime = v.ClosingTime,
                               Latitude = v.Latitude,
                               Longitude = v.Longitude,
                               VenueType = v.VenueType,
                               CreatedDate = v.CreatedDate,
                               CommentRating = v.CommentRating
                           }).ToList();
        }

        public Venue GetDetailById(int id)
        {
            return _context.Venues
                           .Where(v => v.Id == id)
                           .Select(v => new Venue
                           {
                               Id = v.Id,
                               Name = v.Name,
                               Photo = v.Photo,
                               Address = v.Address,
                               OpeningTime = v.OpeningTime,
                               ClosingTime = v.ClosingTime,
                               Latitude = v.Latitude,
                               Longitude = v.Longitude,
                               VenueType = v.VenueType,
                               CreatedDate = v.CreatedDate,
                               CommentRating = v.CommentRating
                           }).FirstOrDefault();
        }

        public Venue GetById(int id)
        {
            return _context.Venues
                           .Where(v => v.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Venue venue)
        {
            _context.Venues.Add(venue);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}