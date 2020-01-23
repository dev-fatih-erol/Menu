using Menu.Core.Models;
using Menu.Data;

namespace Menu.Service
{
    public class SuggestionComplaintService : ISuggestionComplaintService
    {
        private readonly MenuContext _context;

        public SuggestionComplaintService(MenuContext context)
        {
            _context = context;
        }

        public void Create(SuggestionComplaint suggestionComplaint)
        {
            _context.SuggestionComplaints.Add(suggestionComplaint);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}