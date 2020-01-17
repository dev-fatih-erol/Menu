using Menu.Core.Models;

namespace Menu.Service
{
    public interface IOrderTableService
    {
        OrderTable GetByTableIdAndVenueId(int tableId, int venueId);

        void Create(OrderTable orderTable);

        void SaveChanges();
    }
}