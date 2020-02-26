using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class WaiterTokenService : IWaiterTokenService
    {
        private readonly MenuContext _context;

        public WaiterTokenService(MenuContext context)
        {
            _context = context;
        }

        public WaiterToken GetByWaiterId(int waiterId)
        {
            return _context.WaiterTokens
                           .Where(o => o.WaiterId == waiterId)
                           .FirstOrDefault();
        }

        public void Create(WaiterToken waiterToken)
        {
            _context.WaiterTokens.Add(waiterToken);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}