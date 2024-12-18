using AutoMapper;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Shared.Dtos.Basket;
using Shared.Dtos.Inventory;
using Shared.Dtos.Order;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Services
{
    public class CheckoutSagaService : ICheckoutSagaService
    {
        private readonly IOrderHttpRepository _orderHttpRepository;
        private readonly IBasketHttpRepository _basketHttpRepository;
        private readonly IInventoryHttpRepository _inventoryHttpRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CheckoutSagaService(IOrderHttpRepository orderHttpRepository, IBasketHttpRepository basketHttpRepository, IInventoryHttpRepository inventoryHttpRepository, IMapper mapper, ILogger logger)
        {
            _orderHttpRepository = orderHttpRepository;
            _basketHttpRepository = basketHttpRepository;
            _inventoryHttpRepository = inventoryHttpRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckoutDto)
        {
            // Get cart from basket from BasketHttpRepository
            _logger.Information($"Start: Get Cart {userName}");
            var cart = await _basketHttpRepository.GetBasket(userName);
            if (cart == null) return false;
            _logger.Information($"End: Get Cart {userName}");

            // Create order
            _logger.Information($"Start: Create Order");
            var order = _mapper.Map<CreateOrderDto>(basketCheckoutDto);
            order.TotalPrice = cart.TotalPrice;
            var orderId = await _orderHttpRepository.CreateOrder(order);
            if (orderId < 0) return false;

            // Get order by orderId
            var addedOrder = await _orderHttpRepository.GetOrder(orderId);
            _logger.Information($"End: Created Order success, Order Id: {orderId} - Document No - {addedOrder.DocumentNo}");

            var inventoryDocumentNos = new List<string>();
            bool result;
            try
            {
                // Sales Items from InventoryHttpRepository
                foreach (var item in cart.Items)
                {
                    _logger.Information($"Start: Sale Item No: {item.ItemNo} - Quantity: {item.Quantity}");

                    var saleOrder = new SalesProductDto(addedOrder.DocumentNo, item.Quantity);
                    saleOrder.SetItemNo(item.ItemNo);

                    var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                    inventoryDocumentNos.Add(documentNo);

                    _logger.Information($"End: Sale Item No: {item.ItemNo} " +
                                    $"- Quantity: {item.Quantity} - Document No: {documentNo}");
                }
                result = await _basketHttpRepository.DeleteBasket(userName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                // Rollback
                await RollbackCheckoutOrder(userName, orderId, inventoryDocumentNos);
                result = false;
            }

            return result;
        }

        private async Task RollbackCheckoutOrder(string userName, long orderId, List<string> inventoryDocumentNos)
        {
            _logger.Information($"Start: RollbackCheckoutOrder for username: {userName}, " +
                            $"order id: {orderId}, " +
                            $"inventory document nos: {String.Join(", ", inventoryDocumentNos)}");

            var deletedDocumentNos = new List<string>();

            _logger.Information("Start: Delete Order Id: {orderId}");
            await _orderHttpRepository.DeleteOrder(orderId);
            _logger.Information("End: Delete Order Id: {orderId}");

            foreach (var item in inventoryDocumentNos)
            {
                await _inventoryHttpRepository.DeleteOrderByDocumentNo(item);
                deletedDocumentNos.Add(item);
            }
            _logger.Information($"End: Deleted Inventory Document Nos: {string.Join(", ", inventoryDocumentNos)}");
        }
    }
}
