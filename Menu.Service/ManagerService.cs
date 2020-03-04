using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class ManagerService : IManagerService
    {
        private readonly MenuContext _context;

        public ManagerService(MenuContext context)
        {
            _context = context;
        }

        public Manager GetByUsernameAndPassword(string username, string password)
        {
            return _context.Managers.Where(m => m.Username == username && m.Password == password).FirstOrDefault();
        }

        public void Create(Manager manager)
        {
            _context.Managers.Add(manager);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}