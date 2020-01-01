using System;

namespace Menu.Core.Models
{
    public class Rate
    {
        public int Id { get; set; }

        public byte Speed { get; set; }

        public byte Waiter { get; set; }

        public byte Flavor { get; set; }

        public DateTime CreatedDate { get; set; }


        public int UserId { get; set; }

        public virtual User User { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }
    }
}