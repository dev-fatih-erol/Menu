using System.Collections.Generic;
using System.Linq;
using Menu.Core.Enums;
using Menu.Core.Models;

namespace Menu.Api.Models
{
    public static class CityMapper
    {
        public static CityDto ToCityDto(this City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Name = city.Name
            };
        }

        public static IEnumerable<CityDto> ToCityDto(this IEnumerable<City> cities)
        {
            return cities.Select(c => c.ToCityDto());
        }
    }

    public static class VenueMapper
    {
        public static VenueDto ToVenueDto(this Venue venue)
        {
            return new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Latitude = venue.Latitude,
                Longitude = venue.Longitude,
                VenueType = venue.VenueType.ToVenueType()
            };
        }

        public static IEnumerable<VenueDto> ToVenueDto(this IEnumerable<Venue> venues)
        {
            return venues.Select(v => v.ToVenueDto());
        }

        private static string ToVenueType(this VenueType venueType)
        {
            return venueType switch
            {
                VenueType.Cafe => "Cafe",
                VenueType.Restaurant => "Restaurant",
                _ => null,
            };
        }
    }

    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static IEnumerable<CategoryDto> ToCategoryDto(this IEnumerable<Category> categories)
        {
            return categories.Select(c => c.ToCategoryDto());
        }
    }
}