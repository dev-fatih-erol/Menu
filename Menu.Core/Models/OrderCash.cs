using System;

namespace Menu.Core.Models
{
    public class OrderCash
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int CashId { get; set; }

        public virtual Cash Cash { get; set; }


        public int OrderTableId { get; set; }

        public virtual OrderTable OrderTable { get; set; }
    }
}