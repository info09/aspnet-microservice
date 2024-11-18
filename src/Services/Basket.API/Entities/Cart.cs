namespace Basket.API.Entities
{
    public class Cart
    {
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
        public void SetFullName(string fullName) => FullName = fullName;
        public List<CartItem> Items { get; set; }
        public Cart() { }
        public Cart(string userName)
        {
            UserName = userName;
        }
        public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
        public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.UtcNow;
        public string? JobId { get; set; }
    }
}
