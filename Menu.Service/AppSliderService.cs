using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class AppSliderService : IAppSliderService
    {
        private readonly MenuContext _context;

        public AppSliderService(MenuContext context)
        {
            _context = context;
        }

        public List<AppSlider> Get(bool status)
        {
            return _context.AppSliders
                           .Where(a => a.Status == status)
                           .OrderBy(a => a.DisplayOrder)
                           .ToList();
        }

        public void Create(AppSlider appSlider)
        {
            _context.AppSliders.Add(appSlider);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}