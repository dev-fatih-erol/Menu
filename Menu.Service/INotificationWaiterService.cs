using Menu.Core.Models;
using System.Collections.Generic;

namespace Menu.Service
{
    public interface INotificationWaiterService
    {
        NotificationWaiter GetByWaiterIdAndStatus(int waiterId, bool Status, int id);

        List<NotificationWaiter> GetById();

        List<NotificationWaiter> GetByWaiterId(int waiterId, bool Status);

        void Create(NotificationWaiter notificationWaiter);

        void SaveChanges();

    }
}