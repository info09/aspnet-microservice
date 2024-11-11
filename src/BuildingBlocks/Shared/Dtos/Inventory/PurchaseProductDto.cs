using Shared.Enums.Inventory;

namespace Shared.Dtos.Inventory
{
    public record PurchaseProductDto
    {
        public EDocumentType DocumentType => EDocumentType.Purchase;

        private string _itemNo { get; set; }

        public string GetItemNo() => _itemNo;

        public void SetItemNo(string itemNo)
        {
            _itemNo = itemNo;
        }

        public int Quantity { get; set; }
    }
}
