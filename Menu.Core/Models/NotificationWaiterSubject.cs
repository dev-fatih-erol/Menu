using System;
using System.Collections.Generic;

namespace Menu.Core.Models
{
    public class NotificationWaiterSubject
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TableId { get; set; }

        public virtual Table Table { get; set; }



        public virtual List<NotificationWaiter> NotificationWaiter { get; set; }
    }
}
