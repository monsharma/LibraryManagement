using System;
using System.Diagnostics;
using System.Threading;

namespace POC
{
    /// <summary>
    /// Use of Timer, SemaphoreSlim
    /// </summary>
    [Run]
    public class Measure_CPU_Consumption : Runnable
    {
        private static DateTime lastTime = DateTime.MinValue;
        private static TimeSpan lastTotalProcessorTime;
        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;
       
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        volatile static int count = 0;
        
        public override void Run(string[] args)
        {
            SemaphoreSlim ss = new SemaphoreSlim(1);
            string processName = "Taskmgr";
            ss.Wait();
            timer.Elapsed += (s, e) =>
            {
                Process p = Process.GetProcessesByName(processName)[0];
                if (lastTime == null || lastTime == DateTime.MinValue)
                {
                    lastTime = DateTime.Now;
                    lastTotalProcessorTime = p.TotalProcessorTime;
                }
                else
                {
                    curTime = DateTime.Now;
                    curTotalProcessorTime = p.TotalProcessorTime;

                    double CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) /
                    curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);

                    Console.WriteLine("{0} CPU: {1:0.0}%", processName, CPUUsage * 100);

                    lastTime = curTime;
                    lastTotalProcessorTime = curTotalProcessorTime;
                }

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
    }
}
