Portainer: http://localhost:9000
user: admin
pass: Admin12345678

Kibana: http://localhost:5601
user: kibana
pass: admin

RabbitMQ: http://localhost:15672
user: guest
pass: guest

API Gateway: http://localhost:5001/
ProductAPI - Quản lý sản phẩm: http://localhost:5002/
CustomerAPI - Quản lý khách hàng: http://localhost:5003/
BasketAPI - Quản lý giỏ hàng: http://localhost:5004/
OrderAPI - Quản lý order: http://localhost:5005/
InventoryAPI - Quản lý kho: http://localhost:5006/
Inventory.Grpc: http://localhost:5007/
Hangfire API: http://localhost:5008/
Customer.Grpc: http://localhost:5009/
Saga.Orchestrator: http://localhost:5010/

Quy trình nghiệp vụ Checkout:
1. Lấy về giỏ hàng (GetBasket)
2. Tạo đơn hàng (CreateOrder)
3. Lấy đơn hàng (GetOrder)
4. Cập nhật Inventory (UpdateInventory)
