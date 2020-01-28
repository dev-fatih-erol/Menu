namespace Menu.Api.Models
{
    public class CreateCheckOutDto
    {
        public int Tip { get; set; }

        public int UsedPoint { get; set; }

        public int PaymentMethodId { get; set; }
    }
}