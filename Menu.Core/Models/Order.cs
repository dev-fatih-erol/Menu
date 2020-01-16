using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int WaiterId { get; set; }

        public int OrderStatusId { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public int UserId { get; set; }

        public virtual User User { get; set; }


        public int TableId { get; set; }

        public virtual Table Table { get; set; }


        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
}