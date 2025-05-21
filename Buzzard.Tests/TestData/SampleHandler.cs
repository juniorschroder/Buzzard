using Buzzard.Interfaces;

namespace Buzzard.Tests.TestData;

public class SampleHandler : IHandler<SampleRequest, string>
{
    public Task<string> HandleAsync(SampleRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"Handled: {request.Input}");
    }
}
