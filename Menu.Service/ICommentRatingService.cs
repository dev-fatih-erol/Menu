using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICommentRatingService
    {
        List<CommentRating> GetByVenueId(int venueId);

        void Create(CommentRating commentRating);

        void SaveChanges();
    }
}