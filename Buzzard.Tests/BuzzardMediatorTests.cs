using System.Reflection;
using Buzzard.Core;
using Buzzard.Enums;
using Buzzard.Exceptions;
using Buzzard.Interfaces;
using Buzzard.Tests.TestData;
using Buzzard.Tests.TestHandlers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Buzzard.Tests;

public class BuzzardMediatorTests
{
    private readonly IServiceProvider _serviceProvider;

    public BuzzardMediatorTests()
    {
        var services = new ServiceCollection();
        services.AddTransient<IHandler<SampleRequest, string>, SampleHandler>();
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task SendAsync_Should_Invoke_Handler_And_Return_Result()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IBuzzardMediator>();
        var request = new SampleRequest { Input = "Test" };

        // Act
        var result = await mediator.SendAsync(request);

        // Assert
        result.Should().Be("Handled: Test");
    }

    [Fact]
    public async Task SendAsync_Should_Throw_When_Handler_Not_Registered()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IBuzzardMediator>();

        var request = new SampleRequest { Input = "MissingHandler" };

        // Act
        Func<Task> act = async () => await mediator.SendAsync(request);

        // Assert
        await act.Should()
            .ThrowAsync<HandlerNotFoundException>()
            .WithMessage("*SampleRequest*");
    }
    
    [Fact]
    public async Task SendAsync_Should_Propagate_Handler_Exception()
    {
        var services = new ServiceCollection();
        services.AddTransient<IHandler<SampleRequest, string>, ThrowingHandler>();
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IBuzzardMediator>();

        Func<Task> act = async () => await mediator.SendAsync(new SampleRequest());

        var exception = await act.Should().ThrowAsync<TargetInvocationException>();

        exception.WithInnerException(typeof(InvalidOperationException), "Simulated failure");
    }

    [Fact]
    public async Task SendAsync_Should_Respect_CancellationToken()
    {
        var services = new ServiceCollection();
        services.AddTransient<IHandler<SampleRequest, string>, CancellableHandler>();
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IBuzzardMediator>();

        using var cts = new CancellationTokenSource(10); // cancel after 10ms

        var act = async () => await mediator.SendAsync(new SampleRequest(), cts.Token);

        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    [Theory]
    [InlineData(PublishStrategy.Parallel)]
    [InlineData(PublishStrategy.Sequential)]
    [InlineData(PublishStrategy.ParallelWhenAll)]
    public async Task PublishAsync_Should_Invoke_All_NotificationHandlers(PublishStrategy publishStrategy)
    {
        FirstNotificationHandler.Called = false;
        SecondNotificationHandler.Called = false;

        var services = new ServiceCollection();
        services.AddTransient<INotificationHandler<TestNotification>, FirstNotificationHandler>();
        services.AddTransient<INotificationHandler<TestNotification>, SecondNotificationHandler>();
        services.AddTransient<IBuzzardMediator, BuzzardMediator>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IBuzzardMediator>();

        await mediator.PublishAsync(new TestNotification(), publishStrategy);

        FirstNotificationHandler.Called.Should().BeTrue();
        SecondNotificationHandler.Called.Should().BeTrue();
    }
}