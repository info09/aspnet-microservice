using Contracts.Services;
using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using Serilog;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders
{
    public class OrdersDomainHandler : INotificationHandler<OrderCreatedEvent>, INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger _logger;
        private readonly ISmtpEmailService _smtpEmailService;

        public OrdersDomainHandler(ILogger logger, ISmtpEmailService smtpEmailService)
        {
            _logger = logger;
            _smtpEmailService = smtpEmailService;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Ordering Domain Event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information("Ordering Domain Event: {DomainEvent}", notification.GetType().Name);

            var emailRequest = new MailRequest
            {
                ToAddress = notification.EmailAddress,
                Body = $"Your order detail. " +
                   $"<p> Order Id: {notification.DocumentNo}</p>" +
                   $"<p> Total: {notification.TotalPrice}</p>",
                Subject = $"Hello {notification.FullName}, your order was created"
            };

            try
            {
                _smtpEmailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent Created Order to email {notification.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {notification.Id} failed due to an error with the email service: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
