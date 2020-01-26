using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class VenuePaymentMethodService : IVenuePaymentMethodService
    {
        private readonly MenuContext _context;

        public VenuePaymentMethodService(MenuContext context)
        {
            _context = context;
        }

        public VenuePaymentMethod GetById(int id)
        {
            return _context.VenuePaymentMethods
                           .Where(v => v.PaymentMethod.Id == id)
                           .Select(v => new VenuePaymentMethod {
                               Id = v.Id,
                               CreatedDate = v.CreatedDate,
                               VenueId = v.VenueId,
                               Venue = v.Venue,
                               PaymentMethodId = v.PaymentMethodId,
                               PaymentMethod = v.PaymentMethod
                           }).FirstOrDefault();
        }

        public void Create(VenuePaymentMethod venuePaymentMethod)
        {
            _context.VenuePaymentMethods.Add(venuePaymentMethod);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}