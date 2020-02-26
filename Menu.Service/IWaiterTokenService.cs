using Menu.Core.Models;

namespace Menu.Service
{
    public interface IWaiterTokenService
    {
        WaiterToken GetByWaiterId(int waiterId);

        void Create(WaiterToken waiterToken);

        void SaveChanges();
    }
}