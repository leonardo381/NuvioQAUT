using Framework.Diagnostics;
using Framework.Diagnostics.Exceptions;
using System;
using System.Threading.Tasks;

namespace Framework.Core
{
    /// <summary>
    /// Executes actions using a RetryPolicy. This is action-level retry (click/fill/wait),
    /// NOT "rerun the whole test".
    /// </summary>
    public sealed class RetryHandler
    {
        private readonly RetryPolicy _policy;

        public RetryHandler(RetryPolicy? policy = null)
        {
            _policy = policy ?? RetryPolicy.Default;
        }

        public async Task ExecuteAsync(string operation, Func<Task> action)
        {
            Exception? last = null;

            for (int attempt = 1; attempt <= _policy.Attempts; attempt++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    last = ex;

                    if (attempt == _policy.Attempts)
                        break;

                    if (_policy.Delay > TimeSpan.Zero)
                        await Task.Delay(_policy.Delay);
                }
            }

            throw new RetryExceededException(operation, _policy.Attempts, _policy.Delay, last);
        }

        public async Task<T> ExecuteAsync<T>(string operation, Func<Task<T>> action)
        {
            Exception? last = null;

            for (int attempt = 1; attempt <= _policy.Attempts; attempt++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    last = ex;

                    if (attempt == _policy.Attempts)
                        break;

                    if (_policy.Delay > TimeSpan.Zero)
                        await Task.Delay(_policy.Delay);
                }
            }

            throw new RetryExceededException(operation, _policy.Attempts, _policy.Delay, last);
        }
    }
}