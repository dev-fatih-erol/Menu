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

        public CommentRating GetByUserIdAndOrderTableId(int userId, int orderTableId)
        {
            return _context.CommentRatings
                           .Where(c => c.OrderCash.OrderTable.User.Id == userId &&
                                       c.OrderCash.OrderTable.Id == orderTableId)
                           .FirstOrDefault();
        }

        public List<CommentRating> GetByVenueId(int venueId)
        {
            return _context.CommentRatings
                           .Where(c => c.VenueId == venueId)
                           .OrderByDescending(c => c.CreatedDate)
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
                           }).ToList();
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