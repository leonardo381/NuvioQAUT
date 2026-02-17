using System;

namespace Framework.Diagnostics.Exceptions
{
    /// <summary>
    /// Thrown when a wait condition times out (useful to distinguish from other failures).
    /// </summary>
    public class TimeoutExceededException : FrameworkException
    {
        public string Operation { get; }
        public TimeSpan Timeout { get; }

        public TimeoutExceededException(string operation, TimeSpan timeout, Exception? inner = null)
            : base($"Timeout exceeded during '{operation}' after {timeout}.", inner)
        {
            Operation = operation;
            Timeout = timeout;
        }

        public override string ToString()
        {
            return
$@"{Message}
Operation: {Operation}
Timeout: {Timeout}
Inner:
{InnerException}";
        }
    }
}