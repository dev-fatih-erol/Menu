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

        public Product GetDetailById(int id)
        {
            return _context.Products
                           .Where(p => p.Id == id)
                           .Select(p => new Product
                           {
                               Id = p.Id,
                               Code = p.Code,
                               Name = p.Name,
                               Photo = p.Photo,
                               Description = p.Description,
                               Price = p.Price,
                               OpeningTime = p.OpeningTime,
                               ClosingTime = p.ClosingTime,
                               DisplayOrder = p.DisplayOrder,
                               CreatedDate = p.CreatedDate,
                               CategoryId = p.CategoryId,
                               Option = p.Option
                                         .Select(o => new Option
                                         {
                                             Id = o.Id,
                                             Title = o.Title,
                                             OptionType = o.OptionType,
                                             CreatedDate = o.CreatedDate,
                                             ProductId = o.ProductId,
                                             OptionItem = o.OptionItem
                                         }).ToList()
                           }).FirstOrDefault();
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            return _context.Products
                           .Where(p => p.CategoryId == categoryId)
                           .OrderBy(p => p.DisplayOrder)
                           .ToList();
        }

        public Product GetById(int id, int venueId)
        {
            return _context.Products
                           .Where(p => p.Id == id && p.Category.Venue.Id == venueId)
                           .FirstOrDefault();
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