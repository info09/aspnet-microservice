namespace Shared.Dtos.Inventory
{
    public class SalesOrderDto
    {
        public string OrderNo { get; set; }
        public List<SaleItemDto> SaleItem { get; set; }
    }
}
