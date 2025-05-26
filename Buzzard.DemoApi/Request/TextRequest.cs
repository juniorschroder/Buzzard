using Buzzard.Interfaces;

namespace Buzzard.DemoApi.Request;

public class TextRequest : IRequest<string>
{
    public string? Text { get; set; }
}