using System;
using System.Collections.Generic;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedDate { get; set; }


        public int WaiterId { get; set; }

        public virtual Waiter Waiter { get; set; }


        public int UserId { get; set; }

        public virtual User User { get; set; }


        public int OrderTableId { get; set; }

        public virtual OrderTable OrderTable { get; set; }


        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
}