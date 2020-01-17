using System;

namespace Menu.Core.Models
{
    public class OrderWaiter
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int WaiterId { get; set; }

        public virtual Waiter Waiter { get; set; }


        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}