using System;
using System.Collections.Generic;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class Table
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TableStatus TableStatus { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public virtual List<OrderTable> OrderTable { get; set; }


        public virtual List<TableWaiter> TableWaiter { get; set; }
    }
}