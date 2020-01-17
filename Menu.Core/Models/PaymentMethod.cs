using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string Text { get; set; }


        public virtual List<VenuePaymentMethod> VenuePaymentMethod { get; set; }
    }
}