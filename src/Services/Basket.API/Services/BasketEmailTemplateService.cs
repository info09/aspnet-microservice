using Basket.API.Services.Interfaces;

namespace Basket.API.Services
{
    public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public BasketEmailTemplateService()
        {
        }

        public string GenerateReminderCheckoutOrderEmail(string userName, string checkoutUrl = "baskets")
        {
            var _checkoutUrl = $"http://localhost:5001/{checkoutUrl}/checkout";
            var emailText = ReadTemplate("reminder-checkout-order");
            var emailReplacedText = emailText.Replace("[userName]", userName).Replace("[checkoutUrl]", _checkoutUrl);

            return emailReplacedText;
        }
    }
}
