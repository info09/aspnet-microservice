namespace Shared.Dtos.ScheduledJob
{
    public record ReminderCheckoutOrderDto(string email, string subject, string emailContent, DateTimeOffset enqueueAt);
}

