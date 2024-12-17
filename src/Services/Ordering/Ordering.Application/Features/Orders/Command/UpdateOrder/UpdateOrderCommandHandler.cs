using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.Dtos.Order;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Command.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private const string MethodName = "UpdateOrderCommandHandler";
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Order), request.Id);
            _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");

            orderEntity = _mapper.Map(request, orderEntity);
            await _orderRepository.UpdateAsync(orderEntity);

            _logger.Information($"Order {request.Id} was successfully updated.");
            var result = _mapper.Map<OrderDto>(orderEntity);

            _logger.Information($"END: {MethodName} - Order: {request.Id}");
            return new ApiSuccessResult<OrderDto>(result);
        }
    }
}
