using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class CommentRatingService : ICommentRatingService
    {
        private readonly MenuContext _context;

        public CommentRatingService(MenuContext context)
        {
            _context = context;
        }

        public List<CommentRating> GetByVenueId(int venueId)
        {
            return _context.CommentRatings
                           .Where(c => c.VenueId == venueId)
                           .Select(c => new CommentRating
                           {
                               Id = c.Id,
                               Text = c.Text,
                               Speed = c.Speed,
                               Waiter = c.Waiter,
                               Flavor = c.Flavor,
                               CreatedDate = c.CreatedDate,
                               UserId = c.UserId,
                               User = c.User,
                               VenueId = c.VenueId
                           })
                           .OrderByDescending(c => c.CreatedDate)
                           .ToList();
        }

        public void Create(CommentRating commentRating)
        {
            _context.CommentRatings.Add(commentRating);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}