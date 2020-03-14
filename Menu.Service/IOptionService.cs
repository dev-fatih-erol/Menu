using System.Collections.Generic;
using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOptionService
    {
        List<Option> GetByProductId(int productId);

        Option GetById(int id);

        void Create(Option option);

        void SaveChanges();
    }
}