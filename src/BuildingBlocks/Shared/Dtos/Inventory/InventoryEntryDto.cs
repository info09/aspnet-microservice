using Shared.Enums.Inventory;

namespace Shared.Dtos.Inventory
{
    public record InventoryEntryDto
    {
        public string Id { get; set; } = string.Empty;

        public EDocumentType DocumentType { get; set; }

        public string DocumentNo { get; set; } = string.Empty;

        public string ItemNo { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string ExternalDocumentNo { get; set; } = string.Empty;
    }
}
