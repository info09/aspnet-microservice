using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.Dtos.Order;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersByUserNameHandler : IRequestHandler<GetOrdersByUserNameQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public GetOrdersByUserNameHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger;
        }
        private const string MethodName = "GetOrdersQueryHandler";
        public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.Username}");

            var orderEntities = await _orderRepository.GetOrdersByUserName(request.Username);
            //var orderList = _mapper.Map<List<OrderDto>>(orderEntities);
            var orderList = orderEntities.Select(i => new OrderDto()
            {
                Id = i!.Id,
                DocumentNo = i.DocumentNo.ToString(),
                EmailAddress = i.EmailAddress,
                FirstName = i.FirstName,
                InvoiceAddress = i.InvoiceAddress,
                LastName = i.LastName,
                ShippingAddress = i.ShippingAddress,
                Status = i.Status,
                TotalPrice = i.TotalPrice,
                UserName = i.UserName,
            }).ToList();

            _logger.Information($"END: {MethodName} - Username: {request.Username}");

            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
    }
}
