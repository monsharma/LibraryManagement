using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace POC
{
    [Run]
    public class Measure_Process_Memory_Consumption : Runnable
    {
        volatile static int count = 0;
        public override void Run(string[] args)
        {
            SemaphoreSlim ss = new SemaphoreSlim(1);
            string processName = "Taskmgr";
            ss.Wait();
            using (var timer = new System.Timers.Timer(1000))
            {
                timer.Elapsed += (s, e) =>
                    {
                        Process p = Process.GetProcessesByName(processName)[0];
                        PerformanceCounter counter = new PerformanceCounter();
                        counter.CounterName = "Working Set - Private";
                        counter.CategoryName = "Process";
                        counter.InstanceName = p.ProcessName;

                        int memsize = Convert.ToInt32(counter.NextValue());
                        Console.WriteLine($"  WorkingSet64     : {BToMB(memsize)} MB");
                        Console.WriteLine();

                        counter.Dispose();

                        if (++count == 20)
                        {
                            timer.Stop();
                            ss.Release();
                        }
                    };
                timer.Start();
                ss.Wait();
                ss.Release();
            }

            double BToMB(long _bytes) => _bytes / Convert.ToDouble(1024 * 1024);
        }
    }
}
