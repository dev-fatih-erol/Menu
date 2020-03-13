using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.TableViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Lütfen Masa adını girin")]
        public string Name { get; set; }

        public string TableStatus { get; set; }
    }
}