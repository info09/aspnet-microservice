using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities
{
    public class BasketCheckout
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;
        private string _invoiceAddress = string.Empty;
        public string? InvoiceAddress
        {
            get => _invoiceAddress;
            set => _invoiceAddress = value ?? ShippingAddress;
        }
    }
}
