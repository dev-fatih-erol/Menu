using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOptionItemService
    {
        OptionItem GetById(int id, int optionId);

        OptionItem GetById(int id);

        void Create(OptionItem optionItem);

        void SaveChanges();
    }
}