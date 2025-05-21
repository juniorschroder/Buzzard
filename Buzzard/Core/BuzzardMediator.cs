using Buzzard.Enums;
using Buzzard.Exceptions;
using Buzzard.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Buzzard.Core;

public class BuzzardMediator : IBuzzardMediator
{
    private readonly IServiceProvider _serviceProvider;

    public BuzzardMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);
        if (handler is null)
            throw new HandlerNotFoundException(requestType);

        var method = handlerType.GetMethod("HandleAsync");
        if (method == null)
            throw new InvalidOperationException($"Method 'HandleAsync' not found in handler for {requestType.Name}");

        return (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;
    }

    public async Task PublishAsync<TNotification>(
        TNotification notification, PublishStrategy strategy = PublishStrategy.Sequential,
        CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        switch (strategy)
        {
            case PublishStrategy.Parallel:
                var tasks = handlers.Select(handler => Task.Run(
                    () => SafeInvoke(handler, notification, cancellationToken), cancellationToken));
                await Task.WhenAll(tasks);
                break;

            case PublishStrategy.ParallelWhenAll:
                var allTasks = handlers.Select(handler =>
                    handler.HandleAsync(notification, cancellationToken));
                await Task.WhenAll(allTasks);
                break;

            default:
                foreach (var handler in handlers)
                {
                    await SafeInvoke(handler, notification, cancellationToken);
                }
                break;
        }
    }

    private static async Task SafeInvoke<TNotification>(
        INotificationHandler<TNotification> handler,
        TNotification notification,
        CancellationToken cancellationToken)
        where TNotification : INotification
    {
        try
        {
            await handler.HandleAsync(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Notification handler failed: {ex.Message}");
        }
    }

}