using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OptionService : IOptionService
    {
        private readonly MenuContext _context;

        public OptionService(MenuContext context)
        {
            _context = context;
        }

        public List<Option> GetByProductId(int productId)
        {
            return _context.Options
                           .Where(o => o.ProductId == productId)
                           .Select(o => new Option
                           {
                               Id = o.Id,
                               Title = o.Title,
                               CreatedDate = o.CreatedDate,
                               OptionType = o.OptionType,
                               OptionItem = o.OptionItem
                           })
                           .ToList();
        }

        public Option GetById(int id)
        {
            return _context.Options
                           .Where(o => o.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Option option)
        {
            _context.Options.Add(option);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}