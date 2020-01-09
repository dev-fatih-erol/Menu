using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IProductService
    {
        List<Product> GetByIds(int[] ids);

        Product GetDetailById(int id);

        List<Product> GetByCategoryId(int categoryId);

        Product GetById(int id);

        void Create(Product product);

        void SaveChanges();
    }
}