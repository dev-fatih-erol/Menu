using Menu.Core.Models;
using Menu.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Menu.Service
{
    public class NotificationWaiterSubjectService : INotificationWaiterSubjectService
    {
        private readonly MenuContext _context;

        public NotificationWaiterSubjectService(MenuContext context)
        {
            _context = context;
        }

        public NotificationWaiterSubject GetByTestId(bool Status, int id)
        {
            return _context.NotificationWaiterSubjects
                           .Where(w => w.Status == Status && w.Id == id).FirstOrDefault();
        }
        public List<NotificationWaiterSubject> GetById(bool Status)
        {
            return _context.NotificationWaiterSubjects
                           .Where(w => w.Status == Status)
             .Select(o => new NotificationWaiterSubject
             {
                 Id = o.Id,
                 Status = o.Status,
                 CreatedDate = o.CreatedDate,
                 Body = o.Body,
                 Title = o.Title,
                 Type = o.Type,
                 TableId = o.TableId,
                 NotificationWaiter = o.NotificationWaiter.Select(x => new NotificationWaiter
                 {

                 }).ToList(),

             }).ToList();
        }



        public void Create(NotificationWaiterSubject notificationWaiterSubject)
        {
            _context.NotificationWaiterSubjects.Add(notificationWaiterSubject);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}