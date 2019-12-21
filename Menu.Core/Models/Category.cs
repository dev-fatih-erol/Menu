using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public virtual List<Product> Product { get; set; }
    }
}