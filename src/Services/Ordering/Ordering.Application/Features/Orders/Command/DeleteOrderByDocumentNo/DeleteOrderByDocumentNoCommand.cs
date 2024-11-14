using MediatR;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Command.DeleteOrderByDocumentNo
{
    public class DeleteOrderByDocumentNoCommand : IRequest<ApiResult<bool>>
    {
        public string DocumentNo { get; set; }

        public DeleteOrderByDocumentNoCommand(string documentNo)
        {
            DocumentNo = documentNo;
        }
    }
}
