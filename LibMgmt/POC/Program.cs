
namespace POC
{
    
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Use of reflection
    /// </summary>


    public sealed class RunAttribute : Attribute { }
    public abstract class Runnable
    {
        public abstract void Run(string[] args);
    }
    /// <summary>
    /// Do not make any change here
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Stopwatch sw = new Stopwatch();
            var runables = p.GetType().Assembly.GetExportedTypes()
                .Where(x => x.BaseType == typeof(Runnable));

            foreach (var r in runables)
            {
                var ca = r.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(RunAttribute));
                if (ca != null)
                {
                    try
                    {
                        Console.WriteLine($"Started running :  {r.Name}\n");
                        var o = (Runnable)Activator.CreateInstance(r);
                        sw.Start();
                        o?.Run(args);
                    }
                    finally
                    {
                        sw.Stop();
                        Console.WriteLine($"\nFinished running :  {r.Name} Completed in {sw.ElapsedMilliseconds} ms \n");
                    }
                }
            }

        }
    }
}
