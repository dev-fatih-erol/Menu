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

        public List<TableWaiter> GetByTableId(int tableId)
        {
            return _context.TableWaiters
                           .Where(t => t.TableId == tableId)
                           .Select(t => new TableWaiter
                           {
                               Id = t.Id,
                               CreatedDate = t.CreatedDate,
                               WaiterId = t.WaiterId,
                               Waiter = new Waiter {
                                   WaiterToken = t.Waiter.WaiterToken
                               },
                               TableId = t.TableId,
                               Table = t.Table
                           }).ToList();
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

        public void Delete(TableWaiter tableWaiter)
        {
            _context.TableWaiters.Remove(tableWaiter);
        }

        public void Create(TableWaiter tableWaiter)
        {
            _context.TableWaiters.Add(tableWaiter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}