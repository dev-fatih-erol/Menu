using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenuePaymentMethodService
    {
        VenuePaymentMethod GetById(int id);

        void Create(VenuePaymentMethod venuePaymentMethod);

        void SaveChanges();
    }
}