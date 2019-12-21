using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICityService
    {
        City GetById(int id);

        List<City> Get();

        void Create(City city);

        void SaveChanges();
    }
}