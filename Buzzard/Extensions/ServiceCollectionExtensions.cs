using Buzzard.Core;
using Buzzard.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Buzzard.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBuzzard(this IServiceCollection services)
    {
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableToAny(
                typeof(IHandler<,>),
                typeof(INotificationHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
