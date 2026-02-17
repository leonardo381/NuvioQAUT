using System;

namespace Framework.Diagnostics.Exceptions
{
    /// <summary>
    /// Base exception for framework/infrastructure failures (not SUT failures).
    /// </summary>
    public class FrameworkException : Exception
    {
        public FrameworkException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}