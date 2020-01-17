using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class VenuePaymentMethod
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public int PaymentMethodId { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }


        public virtual List<OrderPayment> OrderPayment { get; set; }
    }
}