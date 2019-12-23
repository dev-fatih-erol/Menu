using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class ProductService : IProductService
    {
        private readonly MenuContext _context;

        public ProductService(MenuContext context)
        {
            _context = context;
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            return _context.Products
                           .Where(p => p.CategoryId == categoryId)
                           .OrderBy(p => p.DisplayOrder)
                           .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                           .Where(p => p.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Product product)
        {
            _context.Products.Add(product);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}