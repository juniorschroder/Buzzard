using Buzzard.Interfaces;

namespace Buzzard.Tests.TestData;

public class ThrowingHandler : IHandler<SampleRequest, string>
{
    public Task<string> HandleAsync(SampleRequest request, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Simulated failure");
    }
}
