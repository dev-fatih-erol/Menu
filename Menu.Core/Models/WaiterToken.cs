namespace Menu.Core.Models
{
    public class WaiterToken
    {
        public int Id { get; set; }

        public string Token { get; set; }


        public int WaiterId { get; set; }

        public virtual Waiter Waiter { get; set; }
    }
}