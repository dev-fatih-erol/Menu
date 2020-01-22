using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class OrderPayment
    {
        public int Id { get; set; }

        public int Tip { get; set; }

        public int EarnedPoint { get; set; }

        public int UsedPoint { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenuePaymentMethodId { get; set; }

        public virtual VenuePaymentMethod VenuePaymentMethod { get; set; }


        public int OrderTableId { get; set; }

        public virtual OrderTable OrderTable { get; set; }
    }
}