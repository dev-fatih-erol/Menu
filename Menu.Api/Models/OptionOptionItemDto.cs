using System.Collections.Generic;

namespace Menu.Api.Models
{
    public class OptionOptionItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OptionType { get; set; }

        public List<OptionItemDto> OptionItems { get; set; }
    }
}