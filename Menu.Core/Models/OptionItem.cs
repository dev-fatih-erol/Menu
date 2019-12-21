using System;

namespace Menu.Core.Models
{
    public class OptionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }


        public int OptionId { get; set; }

        public virtual Option Option { get; set; }
    }
}