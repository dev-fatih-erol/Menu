using System;
using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class AppAboutService : IAppAboutService
    {
        private readonly MenuContext _context;

        public AppAboutService(MenuContext context)
        {
            _context = context;
        }

        public List<AppAbout> Get(bool status)
        {
            return _context.AppAbouts
                           .Where(a => a.Status == status)
                           .OrderBy(a => a.DisplayOrder)
                           .ToList();
        }

        public void Create(AppAbout appAbout)
        {
            _context.AppAbouts.Add(appAbout);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}