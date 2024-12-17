using Shared.SeedWorks;

namespace Shared.Dtos.Inventory
{
    public class GetInventoryPagingQuery : PagingRequestParameters
    {
        public string ItemNo() => _itemNo;

        private string _itemNo = string.Empty;

        public void SetItemNo(string itemNo) => _itemNo = itemNo;

        public string? SearchTerm { get; set; }
    }
}
