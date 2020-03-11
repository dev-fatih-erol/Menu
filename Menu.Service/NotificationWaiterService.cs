using Menu.Core.Models;
using Menu.Data;
using System.Collections.Generic;
using System.Linq;

namespace Menu.Service
{
    public class NotificationWaiterService : INotificationWaiterService
    {
        private readonly MenuContext _context;

        public NotificationWaiterService(MenuContext context)
        {
            _context = context;
        }

        public NotificationWaiter GetByWaiterIdAndStatus(int waiterId, bool Status, int id)
        {
            return _context.NotificationWaiters
                           .Where(o => o.WaiterId == waiterId && o.NotificationWaiterSubject.Status == Status && o.NotificationWaiterSubject.Id == id)
                           .Select(o => new NotificationWaiter
                           {
                               Id = o.Id,
                               WaiterId = o.WaiterId,
                               NotificationWaiterSubject = o.NotificationWaiterSubject,
                               Waiter = new Waiter
                               {
                                   Name = o.Waiter.Name,
                                   Surname = o.Waiter.Surname,
                                   Username = o.Waiter.Username,
                                   Id = o.Waiter.Id,

                               }


                           }).FirstOrDefault();
        }


        public List<NotificationWaiter> GetByWaiterId(int waiterId, bool Status)
        {
            return _context.NotificationWaiters
                           .Where(o => o.WaiterId == waiterId && o.NotificationWaiterSubject.Status == Status)
                           .Select(o => new NotificationWaiter
                           {
                               Id = o.Id,
                               WaiterId = o.WaiterId,
                               NotificationWaiterSubject = o.NotificationWaiterSubject

                           }).ToList();
        }

        public List<NotificationWaiter> GetById()
        {
            return _context.NotificationWaiters
                           .OrderBy(o => o.Id)
                           .Select(o => new NotificationWaiter
                           {
                               Id = o.Id,

                               WaiterId = o.WaiterId,
                               Waiter = new Waiter
                               {
                                   Id = o.NotificationWaiterSubject.Id,

                               }

                           }).ToList();
        }

        public void Create(NotificationWaiter notificationWaiter)
        {
            _context.NotificationWaiters.Add(notificationWaiter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}