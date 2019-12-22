using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class OptionItemService : IOptionItemService
    {
        private readonly MenuContext _context;

        public OptionItemService(MenuContext context)
        {
            _context = context;
        }

        public OptionItem GetById(int id)
        {
            return _context.OptionItems
                           .Where(o => o.Id == id)
                           .FirstOrDefault();
        }

        public void Create(OptionItem optionItem)
        {
            _context.OptionItems.Add(optionItem);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}