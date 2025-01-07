namespace Shared.Dtos.Basket
{
    public class CartDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public CartDto()
        {

        }

        public CartDto(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(i => i.ItemPrice * i.Quantity);
    }
}
