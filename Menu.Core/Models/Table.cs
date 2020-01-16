using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Table
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public virtual List<Order> Order { get; set; }
    }
}