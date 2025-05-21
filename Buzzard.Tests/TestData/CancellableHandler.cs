using Buzzard.Interfaces;

namespace Buzzard.Tests.TestData;

public class CancellableHandler : IHandler<SampleRequest, string>
{
    public async Task<string> HandleAsync(SampleRequest request, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken); // Simulates a cancelable operation
        return "Finished";
    }
}
