using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderTableService
    {
        OrderTable GetByTableIdAndVenueId(int tableId, int venueId, int userId, bool isClosed);

        OrderTable GetByTableIdAndVenueId(int tableId, int venueId, bool isClosed);

        void Create(OrderTable orderTable);

        void SaveChanges();
    }
}