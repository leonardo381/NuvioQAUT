using System;

namespace Framework.Diagnostics
{
    /// <summary>
    /// Configuration for retries (how many attempts + delay).
    /// Execution lives in RetryHandler.
    /// </summary>
    public sealed class RetryPolicy
    {
        public int Attempts { get; }
        public TimeSpan Delay { get; }

        public RetryPolicy(int attempts = 3, int delayMs = 300)
        {
            if (attempts < 1) throw new ArgumentOutOfRangeException(nameof(attempts));
            if (delayMs < 0) throw new ArgumentOutOfRangeException(nameof(delayMs));

            Attempts = attempts;
            Delay = TimeSpan.FromMilliseconds(delayMs);
        }

        public static RetryPolicy Default => new RetryPolicy(3, 300);
        public static RetryPolicy Fast => new RetryPolicy(2, 150);
        public static RetryPolicy None => new RetryPolicy(1, 0);
    }
}