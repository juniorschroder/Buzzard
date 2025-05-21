namespace Buzzard.Exceptions;

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(Type requestType)
        : base($"Handler not found for request type: {requestType.FullName}") { }
}