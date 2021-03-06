﻿using System;
using System.Collections.Generic;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Address { get; set; }

        public long OpeningTime { get; set; }

        public long ClosingTime { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public VenueType VenueType { get; set; }

        public DateTime CreatedDate { get; set; }


        public virtual List<Cash> Cash { get; set; }


        public virtual List<Kitchen> Kitchen { get; set; }


        public virtual List<Waiter> Waiter { get; set; }


        public virtual List<Table> Table { get; set; }


        public virtual List<Category> Category { get; set; }


        public virtual List<VenuePaymentMethod> VenuePaymentMethod { get; set; }


        public virtual List<OrderTable> OrderTable { get; set; }


        public virtual List<CommentRating> CommentRating { get; set; }


        public virtual List<Favorite> Favorite { get; set; }


        public virtual List<VenueFeature> VenueFeature { get; set; }


        public int ManagerId { get; set; }

        public virtual Manager Manager { get; set; }
    }
}