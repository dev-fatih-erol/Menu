using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Cash
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public virtual List<OrderCash> OrderCash { get; set; }
    }
}