using Buzzard.DemoApi.Notification;
using Buzzard.Interfaces;

namespace Buzzard.DemoApi.Handlers;

public class MessageNotificationHandler : INotificationHandler<MessageNotification>
{
    public Task HandleAsync(MessageNotification notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($" -> New Message Received: {notification.Message}");
        return Task.CompletedTask;
    }
}