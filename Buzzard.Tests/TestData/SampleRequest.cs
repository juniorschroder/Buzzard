using Buzzard.Interfaces;

namespace Buzzard.Tests.TestData;

public class SampleRequest : IRequest<string>
{
    public string Input { get; set; } = string.Empty;
}
