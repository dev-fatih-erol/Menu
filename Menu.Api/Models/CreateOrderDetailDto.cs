namespace Menu.Api.Models
{
    public class CreateOrderDetailDto
    {
        public int ProductId { get; set; }

        public byte Quantity { get; set; }

        public int[] OptionItems { get; set; }
    }
}