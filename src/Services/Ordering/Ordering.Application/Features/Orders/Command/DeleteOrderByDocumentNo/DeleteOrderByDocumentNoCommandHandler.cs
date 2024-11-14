using MediatR;
using Ordering.Application.Common.Interfaces;
using Serilog;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Command.DeleteOrderByDocumentNo
{
    public class DeleteOrderByDocumentNoCommandHandler : IRequestHandler<DeleteOrderByDocumentNoCommand, ApiResult<bool>>
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderByDocumentNoCommandHandler(ILogger logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<ApiResult<bool>> Handle(DeleteOrderByDocumentNoCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetOrderByDocumentNoAsync(request.DocumentNo);
            if (orderEntity == null)
                return new ApiResult<bool>(true);

            _orderRepository.Delete(orderEntity);
            orderEntity.DeletedOrder();

            await _orderRepository.SaveChangesAsync();

            _logger.Information($"Order {orderEntity.DocumentNo} was successfully deleted.");
            return new ApiResult<bool>(true);
        }
    }
}
