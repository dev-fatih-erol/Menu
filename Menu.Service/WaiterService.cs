using System.Collections.Generic;
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

        public List<Waiter> GetWithTableById(int id)
        {
            return _context.Waiters
                           .Where(w => w.Id == id)
                           .Select(w => new Waiter
                           {
                               Id = w.Id,
                               Name = w.Name,
                               Surname = w.Surname,
                               Username = w.Username,
                               Password = w.Password,
                               TableWaiter = w.TableWaiter.Select(w => new TableWaiter
                               {
                                   Id = w.Id,
                                   CreatedDate = w.CreatedDate,
                                   WaiterId = w.WaiterId,
                                   TableId = w.TableId,
                                   Table = w.Table
                               }).ToList()
                           }).ToList();
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