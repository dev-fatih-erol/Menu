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