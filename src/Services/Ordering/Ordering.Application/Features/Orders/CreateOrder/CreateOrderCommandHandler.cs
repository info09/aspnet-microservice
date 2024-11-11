using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWorks;
using Shared.Services.Email;

namespace Ordering.Application.Features.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CreateOrderCommandHandler(IOrderRepository orderRepository = null, IMapper mapper = null, ILogger logger = null)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");

            var orderEntity = _mapper.Map<Order>(request);
            var addedOrder = await _orderRepository.CreateAsync(orderEntity);
            await _orderRepository.SaveChangesAsync();

            //_logger.Information($"Order {orderEntity.Id} - Document No: {orderEntity.DocumentNo} was successfully created.");


            _logger.Information($"END: {MethodName} - Username: {request.UserName}");

            return new ApiSuccessResult<long>(orderEntity.Id);
        }
    }
}
