using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Serilog;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public GetOrdersHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger;
        }
        private const string MethodName = "GetOrdersQueryHandler";
        public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.Username}");

            var orderEntities = await _orderRepository.GetOrdersByUserName(request.Username);
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

            _logger.Information($"END: {MethodName} - Username: {request.Username}");

            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
    }
}
