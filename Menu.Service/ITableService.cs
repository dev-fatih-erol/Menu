using Menu.Core.Models;

namespace Menu.Service
{
    public interface ITableService
    {
        Table GetById(int id);

        void Create(Table table);

        void SaveChanges();
    }
}