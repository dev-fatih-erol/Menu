using System;

namespace Menu.Core.Models
{
    public class TableWaiter
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int TableId { get; set; }

        public virtual Table Table { get; set; }


        public int WaiterId { get; set; }

        public virtual Waiter Waiter { get; set; }
    }
}