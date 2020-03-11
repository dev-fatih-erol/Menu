using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.OptionViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Lütfen başlık girin")]
        public string Title { get; set; }

        public List<OptionItem> OptionItems { get; set; }
    }

    public class OptionItem
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}