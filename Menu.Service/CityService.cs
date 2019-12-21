using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class CityService : ICityService
    {
        private readonly MenuContext _context;

        public CityService(MenuContext context)
        {
            _context = context;
        }

        public City GetById(int id)
        {
            return _context.Cities
                           .Where(c => c.Id == id)
                           .FirstOrDefault();
        }

        public List<City> Get()
        {
            return _context.Cities.ToList();
        }

        public void Create(City city)
        {
            _context.Cities.Add(city);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}