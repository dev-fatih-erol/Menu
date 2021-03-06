﻿using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string Photo { get; set; }

        public int Point { get; set; }

        public bool IsGuest { get; set; }

        public DateTime CreatedDate { get; set; }


        public int CityId { get; set; }

        public virtual City City { get; set; }


        public virtual List<OrderTable> OrderTable { get; set; }


        public virtual List<CommentRating> CommentRating { get; set; }


        public virtual List<Favorite> Favorite { get; set; }


        public virtual UserToken UserToken { get; set; }
    }
}