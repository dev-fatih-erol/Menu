using System;
using Menu.Core.Enums;

namespace Menu.Core.Models
{
    public class SuggestionComplaint
    {
        public int Id { get; set; }

        public SubjectType SubjectType { get; set; }

        public string Description { get; set; }

        public SuggestionComplaintStatus SuggestionComplaintStatus { get; set; }

        public DateTime CreatedDate { get; set; }


        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}