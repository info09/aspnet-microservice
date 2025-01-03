using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Dtos.ScheduledJob;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;
        private readonly BackgroundJobHttpService _backgroundJobHttpService;
        private readonly IEmailTemplateService _emailTemplateService;

        public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger, BackgroundJobHttpService backgroundJobHttpService, IEmailTemplateService emailTemplateService)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;
            _backgroundJobHttpService = backgroundJobHttpService;
            _emailTemplateService = emailTemplateService;
        }
        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            await DeleteReminderCheckoutOrder(userName);
            try
            {
                _logger.Information($"BEGIN: DeleteBasketFromUserName {userName}");
                await _redisCacheService.RemoveAsync(userName);
                _logger.Information($"END: DeleteBasketFromUserName {userName}");

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Error DeleteBasketFromUserName: " + e.Message);
                throw;
            }
        }

        public async Task<Cart?> GetBasketByUserName(string userName)
        {
            var basket = await _redisCacheService.GetStringAsync(userName);

            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions? options = null)
        {
            await DeleteReminderCheckoutOrder(cart.UserName);
            _logger.Information($"BEGIN: UpdateBasket for {cart.UserName}");

            if (options != null)
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart), options);
            else
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart));

            _logger.Information($"END: UpdateBasket for {cart.UserName}");

            try
            {
                await TriggerSendEmailReminderCheckout(cart);
            }
            catch (Exception e)
            {
                _logger.Error($"UpdateBasket: {e.Message}");
            }

            return await GetBasketByUserName(cart.UserName);
        }

        private async Task TriggerSendEmailReminderCheckout(Cart cart)
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.UserName);
            var model = new ReminderCheckoutOrderDto(cart.EmailAddress!, "Reminder checkout", emailTemplate, DateTimeOffset.UtcNow.AddSeconds(30));

            var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/send-email-reminder-checkout-order";
            var response = await _backgroundJobHttpService.Client.PostAsJson(uri, model);
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                var jobId = await response.ReadContentAs<string>();
                if (!string.IsNullOrEmpty(jobId))
                {
                    cart.JobId = jobId;
                    await _redisCacheService.SetStringAsync(cart.UserName,
                        _serializeService.Serialize(cart));
                }
            }
        }

        private async Task DeleteReminderCheckoutOrder(string username)
        {
            var cart = await GetBasketByUserName(username);
            if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

            var jobId = cart.JobId;
            var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/delete/jobId/{jobId}";
            await _backgroundJobHttpService.Client.DeleteAsync(uri);
            _logger.Information($"DeleteReminderCheckoutOrder:Deleted JobId: {jobId}");
        }
    }
}
