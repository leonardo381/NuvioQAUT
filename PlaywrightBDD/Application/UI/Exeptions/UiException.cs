using System;

namespace Application.UI.Exceptions
{
    /// <summary>
    /// Represents a failed UI interaction (click/fill/wait/etc).
    /// Adds selector + operation + url for fast debugging.
    /// </summary>
    public sealed class UiException : Exception
    {
        public string Operation { get; }
        public string Selector { get; }
        public string? Url { get; }

        public UiException(
            string operation,
            string selector,
            string? url,
            Exception inner)
            : base($"UI operation failed: {operation} | Selector: {selector} | Url: {url}", inner)
        {
            Operation = operation;
            Selector = selector;
            Url = url;
        }

        public override string ToString()
        {
            return
$@"{Message}
Operation: {Operation}
Selector: {Selector}
Url: {Url}
Inner:
{InnerException}";
        }
    }
}