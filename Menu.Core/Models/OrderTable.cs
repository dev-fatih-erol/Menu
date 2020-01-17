using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class OrderTable
    {
        public int Id { get; set; }

        public bool IsClosed { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public int TableId { get; set; }

        public virtual Table Table { get; set; }


        public virtual List<Order> Order { get; set; }


        public virtual List<OrderPayment> OrderPayment { get; set; }
    }
}