using Buzzard.Interfaces;

namespace Buzzard.DemoApi.Notification;

public class MessageNotification : INotification
{
    public string Message { get; set; } = string.Empty;
}