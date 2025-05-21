namespace Buzzard.Exceptions;

public class NotificationHandlerException : Exception
{
    public IReadOnlyCollection<Exception> InnerExceptions { get; }

    public NotificationHandlerException(IEnumerable<Exception> innerExceptions)
        : base("One or more notification handlers failed.")
    {
        InnerExceptions = innerExceptions.ToList().AsReadOnly();
    }
}
