using System;

namespace Menu.Api.Models
{
    public class VenueDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Address { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string VenueType { get; set; }
    }
}