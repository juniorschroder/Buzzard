using Buzzard.Interfaces;
using Buzzard.Tests.TestData;

namespace Buzzard.Tests.TestHandlers;

public class SecondNotificationHandler : INotificationHandler<TestNotification>
{
    public static bool Called = false;
    public Task HandleAsync(TestNotification notification, CancellationToken cancellationToken = default)
    {
        Called = true;
        return Task.CompletedTask;
    }
}