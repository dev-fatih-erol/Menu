using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public long OpeningTime { get; set; }

        public long ClosingTime { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }


        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }


        public virtual List<Option> Option { get; set; }
    }
}