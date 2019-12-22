using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly MenuContext _context;

        public CategoryService(MenuContext context)
        {
            _context = context;
        }

        public List<Category> GetCategoriesAndProductsByVenueId(int venueId)
        {
            return _context.Categories.Where(c => c.VenueId == venueId)
                                      .OrderBy(c => c.DisplayOrder)
                                      .Select(c => new Category
                                      {
                                          Id = c.Id,
                                          Name = c.Name,
                                          DisplayOrder = c.DisplayOrder,
                                          CreatedDate = c.CreatedDate,
                                          VenueId = c.VenueId,
                                          Product = c.Product
                                          .OrderBy(p => p.DisplayOrder)
                                          .ToList()
                                      }).ToList();
        }

        public List<Category> GetByVenueId(int venueId)
        {
            return _context.Categories
                           .Where(c => c.VenueId == venueId)
                           .OrderBy(c => c.DisplayOrder)
                           .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                           .Where(c => c.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Category category)
        {
            _context.Categories.Add(category);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}