# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2025-05-21

### Added

- Initial release of **Buzzard** 🎉
- Core mediator pattern implementation:
    - `IRequest<TResponse>` and `IHandler<TRequest, TResponse>`
- Support for **notifications**:
    - `INotification` and `INotificationHandler<TNotification>`
    - `PublishAsync` with execution strategies: `Sequential`, `Parallel`, `ParallelWhenAll`
- Exception-safe notification execution (`SafeInvoke`)
- Full unit test coverage for core features
- NuGet metadata and packaging support
- MIT License

---

## [Unreleased]

- 🔄 Placeholder for future features (e.g., pipeline behaviors toggle, logging, validation integration)
