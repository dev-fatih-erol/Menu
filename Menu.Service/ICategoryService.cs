using Menu.Core.Models;

namespace Menu.Service
{
    public interface ICategoryService
    {
        Category GetById(int id);

        void Create(Category category);

        void SaveChanges();
    }
}