using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public virtual List<User> User { get; set; }
    }
}