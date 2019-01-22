using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;

namespace EngMonarchApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int processorCounter = Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(processorCounter, processorCounter);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
