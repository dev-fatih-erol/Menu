using Menu.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Service
{
    public interface INotificationWaiterSubjectService
    {

        NotificationWaiterSubject GetByTestId(bool Status, int id);
        List<NotificationWaiterSubject> GetById(bool Status);

        void Create(NotificationWaiterSubject notificationWaiterSubject);

        void SaveChanges();
    }
}