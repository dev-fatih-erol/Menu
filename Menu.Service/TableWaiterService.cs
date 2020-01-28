using System.Collections.Generic;
using System.Linq;
using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class TableWaiterService : ITableWaiterService
    {
        private readonly MenuContext _context;

        public TableWaiterService(MenuContext context)
        {
            _context = context;
        }

        public List<TableWaiter> GetByWaiterId(int waiterId)
        {
            return _context.TableWaiters
                           .Where(t => t.Waiter.Id == waiterId)
                           .Select(t => new TableWaiter
                           {
                               Id = t.Id,
                               CreatedDate = t.CreatedDate,
                               WaiterId = t.WaiterId,
                               Waiter = t.Waiter,
                               TableId = t.TableId,
                               Table = t.Table
                           }).ToList();
        }

        public TableWaiter GetByTableIdAndWaiterId(int tableId, int waiterId)
        {
            return _context.TableWaiters
                           .Where(o => o.Table.Id == tableId &&
                                       o.Waiter.Id == waiterId)
                           .FirstOrDefault();
        }

        public void Create(TableWaiter tableWaiter)
        {
            throw new System.NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}