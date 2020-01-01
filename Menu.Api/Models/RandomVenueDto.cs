using System;

namespace Menu.Api.Models
{
    public class RandomVenueDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public string VenueType { get; set; }

        public string Address { get; set; }

        public int Rate { get; set; }

        public int CommentCount { get; set; }
    }
}
