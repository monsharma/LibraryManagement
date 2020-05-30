using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace POC
{
    //[Run]
    public class UseOf_SemaphoreSlim : Runnable
    {
        private void SemaphoreLock(SemaphoreSlim semaphoreSlim, Action action, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(cancellationToken);
            try
            {
                action();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public override void Run(string[] args)
        {
            var ss = new SemaphoreSlim(1);
            SemaphoreLock(ss, () => { /* do something */ }, CancellationToken.None);
        }
    }
}
