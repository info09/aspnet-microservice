namespace Shared.Dtos.Inventory
{
    public class CreatedSalesOrderSuccecssDto
    {
        public CreatedSalesOrderSuccecssDto(string documentNo)
        {
            DocumentNo = documentNo;
        }

        public string DocumentNo { get; set; }
    }
}
