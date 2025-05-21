using Buzzard.Enums;

namespace Buzzard.Interfaces;

public interface IBuzzardMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task PublishAsync<TNotification>(
        TNotification notification, PublishStrategy strategy = PublishStrategy.Sequential,
        CancellationToken cancellationToken = default)
        where TNotification : INotification;
}