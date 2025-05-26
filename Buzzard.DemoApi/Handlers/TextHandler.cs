using Buzzard.DemoApi.Request;
using Buzzard.Interfaces;

namespace Buzzard.DemoApi.Handlers;

public class TextHandler : IHandler<TextRequest, string>
{
    public Task<string> HandleAsync(TextRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"A new text are send: {request.Text}");
    }
}