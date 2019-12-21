using System;
using System.Collections.Generic;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class Option
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public OptionType OptionType { get; set; }

        public DateTime CreatedDate { get; set; }


        public int ProductId { get; set; }

        public virtual Product Product { get; set; }


        public virtual List<OptionItem> OptionItem { get; set; }
    }
}