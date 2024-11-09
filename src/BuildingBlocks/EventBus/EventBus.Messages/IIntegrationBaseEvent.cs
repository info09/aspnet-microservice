namespace EventBus.Messages
{
    public interface IIntegrationBaseEvent
    {
        DateTime CreationDate { get; }
        Guid Id { get; }
    }
}
