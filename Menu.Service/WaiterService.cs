using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class WaiterService : IWaiterService
    {
        private readonly MenuContext _context;

        public WaiterService(MenuContext context)
        {
            _context = context;
        }

        public Waiter GetByUsernameAndPassword(string username, string password)
        {
            return _context.Waiters
                           .Where(w =>
                                  w.Username == username &&
                                  w.Password == password)
                           .FirstOrDefault();
        }

        public Waiter GetById(int id)
        {
            return _context.Waiters
                           .Where(w => w.Id == id)
                           .FirstOrDefault();
        }

        public void Create(Waiter waiter)
        {
            _context.Waiters.Add(waiter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}