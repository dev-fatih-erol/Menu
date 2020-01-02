using System;

namespace Menu.Api.Models
{
    public class CommentRatingDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public byte Speed { get; set; }

        public byte Waiter { get; set; }

        public byte Flavor { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserProfileDto User { get; set; }
    }
}