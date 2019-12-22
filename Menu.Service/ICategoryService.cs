using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICategoryService
    {
        List<Category> GetCategoriesAndProductsByVenueId(int venueId);

        List<Category> GetByVenueId(int venueId);

        Category GetById(int id);

        void Create(Category category);

        void SaveChanges();
    }
}