using Shared.Enums.Inventory;

namespace Shared.Dtos.Inventory
{
    public record SalesProductDto(string ExternalDocumentNo, int Quantity)
    {
        public EDocumentType EDocumentType { get; set; } = EDocumentType.Sale;
        public string ItemNo { get; set; }
        public void SetItemNo(string itemNo)
        {
            ItemNo = itemNo;
        }
    }
}
