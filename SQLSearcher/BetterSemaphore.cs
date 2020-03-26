using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLSearcher
{
    /// <summary>
    /// An adapter around the <see cref="SemaphoreSlim"/> class in order to utilize <c>using</c> blocks to better illustrate the locked scope.
    /// </summary>
    class BetterSemaphore : IDisposable
    {

        private SemaphoreSlim _semaphore;

        public class Grant : IDisposable
        {
            protected SemaphoreSlim _semaphore;
            
            public Grant(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }

            public void Dispose()
            {
                _semaphore.Release();
            }
        }

        public BetterSemaphore(int initial, int limit)
        {
            _semaphore = new SemaphoreSlim(initial, limit);
        }

        public Grant Lock()
        {
            _semaphore.Wait();
            return new Grant(_semaphore);
        }

        public async Task<Grant> LockAsync()
        {
            await _semaphore.WaitAsync();
            return new Grant(_semaphore);
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }
}
