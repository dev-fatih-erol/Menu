using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class CashService : ICashService
    {
        private readonly MenuContext _context;

        public CashService(MenuContext context)
        {
            _context = context;
        }

        public Cash GetByUsernameAndPassword(string username, string password)
        {
            return _context.Cashes
                           .Where(k => k.Username == username &&
                                       k.Password == password)
                           .FirstOrDefault();
        }

        public Cash GetById(int id)
        {
            return _context.Cashes.Where(c => c.Id == id)
                                    .Select(c => new Cash
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Username = c.Username,
                                        Password = c.Password,
                                        CreatedDate = c.CreatedDate,
                                        VenueId = c.VenueId,
                                        Venue = c.Venue
                                    }).FirstOrDefault();
        }

        public void Create(Cash cash)
        {
            _context.Cashes.Add(cash);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}