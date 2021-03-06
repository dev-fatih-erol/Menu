﻿using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class Waiter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }


        public int VenueId { get; set; }

        public virtual Venue Venue { get; set; }


        public virtual List<TableWaiter> TableWaiter { get; set; }

        public virtual WaiterToken WaiterToken { get; set; }
    }
}