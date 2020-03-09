using System;
using System.Collections.Generic;

namespace Menu.Business.Models.WaiterViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string CreatedDate { get; set; }

        public IEnumerable<string> Tables { get; set; }
    }
}