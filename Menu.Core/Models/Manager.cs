using System;
namespace Menu.Core.Models
{
    public class Manager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }


        public virtual Venue Venue { get; set; }
    }
}