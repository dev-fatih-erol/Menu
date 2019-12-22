using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;

namespace Menu.Api.Models
{
    public static class CityMapper
    {
        public static CityDto ConvertToDto(this City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Name = city.Name
            };
        }

        public static IEnumerable<CityDto> ConvertToDto(this IEnumerable<City> cities)
        {
            return cities.Select(c => c.ConvertToDto());
        }
    }

    public static class VenueMapper
    {
        public static VenueDto ConvertToDto(this Venue venue)
        {
            return new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Latitude = venue.Latitude,
                Longitude = venue.Longitude
            };
        }

        public static IEnumerable<VenueDto> ConvertToDto(this IEnumerable<Venue> venues)
        {
            return venues.Select(v => v.ConvertToDto());
        }
    }
}