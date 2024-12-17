using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Shared.Dtos.Order;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            var orderDto = _mapper.Map<OrderDto>(order);

            return new ApiSuccessResult<OrderDto>(orderDto);
        }
    }
}
