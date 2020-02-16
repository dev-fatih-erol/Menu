using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenuePaymentMethodService
    {
        VenuePaymentMethod GetByVenueId(int paymentMethodId, int venueId);

        List<VenuePaymentMethod> GetByVenueId(int venueId);

        VenuePaymentMethod GetById(int id);

        void Create(VenuePaymentMethod venuePaymentMethod);

        void SaveChanges();
    }
}