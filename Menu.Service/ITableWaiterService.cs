using Menu.Core.Models;

namespace Menu.Service
{
    public interface ITableWaiterService
    {
        TableWaiter GetByTableIdAndWaiterId(int tableId, int waiterId);

        void Create(TableWaiter tableWaiter);

        void SaveChanges();
    }
}