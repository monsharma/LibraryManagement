// Code to get CPU utilization of a process.


using System;
using System.Diagnostics;
using System.Timers;

namespace ConsoleApp1
{
    class Program
    {
        private static DateTime lastTime;
        private static TimeSpan lastTotalProcessorTime;
        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;

        static Timer timer = new Timer(1000);
        volatile static int count = 0;
        static void Main(string[] args)
        {
            string processName = "Taskmgr";
            timer.Elapsed += (s, e) =>
            {
                Process p = Process.GetProcessesByName(processName)[0];
                if (lastTime == null || lastTime == new DateTime())
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
                    timer.Stop();
            };
            timer.Start();
            Console.ReadKey();
            //timer.Stop();

        }
    }
}
