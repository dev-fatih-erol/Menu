using System.Collections.Generic;

namespace Menu.Business.Models.OptionViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OptionType { get; set; }

        public List<OptionItem> OptionItems { get; set; }
    }
}