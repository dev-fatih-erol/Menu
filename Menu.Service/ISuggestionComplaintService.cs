using Menu.Core.Models;

namespace Menu.Service
{
    public interface ISuggestionComplaintService
    {
        void Create(SuggestionComplaint suggestionComplaint);

        void SaveChanges();
    }
}