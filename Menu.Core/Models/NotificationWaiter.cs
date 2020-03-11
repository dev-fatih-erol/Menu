namespace Menu.Core.Models
{
    public class NotificationWaiter
    {
        public int Id { get; set; }

        public int WaiterId { get; set; }

        public int NotificationWaiterSubjectID { get; set; }

        public virtual Waiter Waiter { get; set; }


        public virtual NotificationWaiterSubject NotificationWaiterSubject { get; set; }

    }
}