using System;

namespace Framework.Diagnostics.Exceptions
{
    /// <summary>
    /// Thrown when retry logic exhausts all attempts.
    /// Wraps the last observed exception as InnerException.
    /// </summary>
    public class RetryExceededException : FrameworkException
    {
        public string Operation { get; }
        public int Attempts { get; }
        public TimeSpan Delay { get; }

        public RetryExceededException(
            string operation,
            int attempts,
            TimeSpan delay,
            Exception? inner = null)
            : base($"Operation '{operation}' failed after {attempts} attempt(s).", inner)
        {
            Operation = operation;
            Attempts = attempts;
            Delay = delay;
        }

        public override string ToString()
        {
            return
$@"{Message}
Operation: {Operation}
Attempts: {Attempts}
Delay: {Delay}
Inner:
{InnerException}";
        }
    }
}