using Menu.Core.Models;

namespace Menu.Service
{
    public interface IProductService
    {
        Product GetById(int id);

        void Create(Product product);

        void SaveChanges();
    }
}