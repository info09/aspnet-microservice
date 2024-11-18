using Basket.API.Services.Interfaces;

namespace Basket.API.Services
{
    public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public BasketEmailTemplateService()
        {
        }

        public string GenerateReminderCheckoutOrderEmail(string userName)
        {
            var checkoutUrl = "http://localhost:5001/baskets/checkout";
            var emailText = ReadTemplate("reminder-checkout-order");
            var emailReplacedText = emailText.Replace("[userName]", userName).Replace("[checkoutUrl]", checkoutUrl);

            return emailReplacedText;
        }
    }
}
