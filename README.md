# 🦅 Buzzard
**Buzzard** is a lightweight, fast, and extensible .NET library that implements the Mediator pattern — similar to [MediatR](https://github.com/jbogard/MediatR) — with full developer control and minimal overhead.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=juniorschroder_Buzzard&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=juniorschroder_Buzzard)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=juniorschroder_Buzzard&metric=bugs)](https://sonarcloud.io/summary/new_code?id=juniorschroder_Buzzard)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=juniorschroder_Buzzard&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=juniorschroder_Buzzard)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=juniorschroder_Buzzard&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=juniorschroder_Buzzard)
[![NuGet](https://img.shields.io/nuget/v/Buzzard.svg?label=NuGet&style=flat-square)](https://www.nuget.org/packages/Buzzard/)

---
## ✨ Features
- Request/Response messaging with `IRequest<TResponse>`
- Fire-and-forget notifications with `INotification`
- Clean, dependency-injection-first design
- No runtime reflection or complex setup
- Designed for performance and testability

## 📦 Installation
Install via NuGet:
```bash
dotnet add package Buzzard
```
## 🛠️ Getting Started

### 1. Configure Services

Register Buzzard in your `Startup.cs` or `Program.cs`:

```csharp
services.AddBuzzard();
```

---
### 2. Create a Request

```csharp
public class GetUserQuery : IRequest<User>
{
    public int Id { get; set; }
}
```

---

### 3. Implement the Handler

```csharp
public class GetUserQueryHandler : IHandler<GetUserQuery, User>
{
    public Task<User> HandleAsync(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Fetch from database or service
        return Task.FromResult(new User { Id = request.Id, Name = "Alice" });
    }
}
```

---

### 4. Send a Request

```csharp
var user = await _buzzardMediator.SendAsync(new GetUserQuery { Id = 1 });
```

---

## 🔔 Notifications

Buzzard supports fire-and-forget notifications that can have multiple handlers.

### 1. Define a Notification

```csharp
public class OrderPlaced : INotification
{
    public int OrderId { get; set; }
}
```

---

### 2. Create Handlers
```csharp
public class EmailHandler : INotificationHandler<OrderPlaced>
{
    public Task HandleAsync(OrderPlaced notification, CancellationToken cancellationToken)
    {
        // Send confirmation email
        return Task.CompletedTask;
    }
}
```

```csharp
public class LogHandler : INotificationHandler<OrderPlaced>
{
    public Task HandleAsync(OrderPlaced notification, CancellationToken cancellationToken)
    {
        // Log the order
        return Task.CompletedTask;
    }
}
```

---

### 3. Publish a Notification

```csharp
await _buzzardMediator.PublishAsync(new OrderPlaced { OrderId = 123 });
```

Optionally choose the publish strategy:

```csharp
await _buzzardMediator.PublishAsync(notification, PublishStrategy.Parallel);         // Background threads
await _buzzardMediator.PublishAsync(notification, PublishStrategy.ParallelWhenAll); // Waits for all
await _buzzardMediator.PublishAsync(notification, PublishStrategy.Sequential);      // Default
```

---

## ✅ Unit Testing
Buzzard makes testing easy. You can test handlers directly:

```csharp
var handler = new GetUserQueryHandler();
var result = await handler.HandleAsync(new GetUserQuery { Id = 1 }, CancellationToken.None);
```

Or mock `IBuzzardMediator` for higher-level tests.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request if you want to help improve Buzzard.

---

## 📄 License

Buzzard is licensed under the [MIT License](LICENSE).