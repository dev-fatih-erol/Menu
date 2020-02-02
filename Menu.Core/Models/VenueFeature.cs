using System;

namespace Menu.Core.Models
{
    public class VenueFeature
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
    }
}