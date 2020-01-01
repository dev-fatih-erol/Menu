using System;
using System.Collections.Generic;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Address { get; set; }

        public long OpeningTime { get; set; }

        public long ClosingTime { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public VenueType VenueType { get; set; }

        public DateTime CreatedDate { get; set; }


        public virtual List<Category> Category { get; set; }
    }
}