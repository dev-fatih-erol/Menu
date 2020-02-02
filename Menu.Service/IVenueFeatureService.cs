using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IVenueFeatureService
    {
        List<VenueFeature> GetByVenueId(int venueId);

        void Create(VenueFeature venueFeature);

        void SaveChanges();
    }
}