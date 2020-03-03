using System;

namespace Menu.Core.Models
{
    public class DayReports
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }
    }
}