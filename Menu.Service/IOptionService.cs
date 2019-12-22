using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOptionService
    {
        Option GetById(int id);

        void Create(Option option);

        void SaveChanges();
    }
}