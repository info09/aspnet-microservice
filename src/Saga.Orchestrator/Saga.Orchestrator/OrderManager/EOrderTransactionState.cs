namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderTransactionState
    {
        NotStarted,
        BasketGot,
        BasketGetFailed,
        BasketDeleted,
        OrderCreated,
        OrderCreateFailed,
        OrderDeleted,
        OrderDeleteFailed,
        OrderGot,
        OrderGetFailed,
        InventoryUpdated,
        InventoryUpdateFailed,
        RollbackInventory,
        InventoryRollback,
        InventoryRollbackFailed,
    }
}
