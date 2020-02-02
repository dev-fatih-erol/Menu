using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class VenueFeatureService : IVenueFeatureService
    {    
        private readonly MenuContext _context;

        public VenueFeatureService(MenuContext context)
        {
            _context = context;
        }

        public List<VenueFeature> GetByVenueId(int venueId)
        {
            return _context.VenueFeatures.Where(v => v.Venue.Id == venueId)
                                         .Select(v => new VenueFeature
                                         {
                                             Id = v.Id,
                                             CreatedDate = v.CreatedDate,
                                             VenueId = v.VenueId,
                                             FeatureId = v.FeatureId,
                                             Feature = v.Feature
                                         })
                                         .ToList();
        }

        public void Create(VenueFeature venueFeature)
        {
            _context.VenueFeatures.Add(venueFeature);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}