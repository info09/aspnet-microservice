namespace Contracts.Sagas.OrderManager
{
    public interface ISagaOrderManager<in TInput, out TOutput> where TInput : class where TOutput : class
    {
        TOutput CreateOrder(TInput input);
        TOutput RollbackOrder(string userName, string documentNo, long orderId);
    }
}
