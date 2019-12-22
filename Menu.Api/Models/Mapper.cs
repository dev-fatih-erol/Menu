using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;

namespace Menu.Api.Models
{
    public static class PersonMapper
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
}