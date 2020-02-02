using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }

        public string Text { get; set; }


        public virtual List<VenueFeature> VenueFeature { get; set; }
    }
}