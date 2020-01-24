using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICommentRatingService
    {
        CommentRating GetByUserIdAndOrderTableId(int userId, int orderTableId);

        List<CommentRating> GetByVenueId(int venueId);

        void Create(CommentRating commentRating);

        void SaveChanges();
    }
}