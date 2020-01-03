using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly MenuContext _context;

        public FavoriteService(MenuContext context)
        {
            _context = context;
        }

        public Favorite GetByUserIdAndVenueId(int userId, int venueId)
        {
            return _context.Favorites
                           .Where(f => f.UserId == userId &&
                                       f.VenueId == venueId)
                           .FirstOrDefault();
        }

        public List<Favorite> GetByUserId(int userId)
        {
            return _context.Favorites
                           .Where(f => f.UserId == userId)
                           .Select(f => new Favorite
                           {
                               Id = f.Id,
                               CreatedDate = f.CreatedDate,
                               UserId = f.UserId,
                               VenueId = f.VenueId,
                               Venue = new Venue
                               {
                                   Id = f.Venue.Id,
                                   Name = f.Venue.Name,
                                   Photo = f.Venue.Photo,
                                   Address = f.Venue.Address,
                                   OpeningTime = f.Venue.OpeningTime,
                                   ClosingTime = f.Venue.ClosingTime,
                                   Latitude = f.Venue.Latitude,
                                   Longitude = f.Venue.Longitude,
                                   VenueType = f.Venue.VenueType,
                                   CreatedDate = f.Venue.CreatedDate,
                                   CommentRating = f.Venue.CommentRating
                               }
                           }).ToList();
        }

        public Favorite GetById(int id)
        {
            return _context.Favorites
                           .Where(f => f.Id == id)
                           .FirstOrDefault();
        }

        public void Delete(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
        }

        public void Create(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}