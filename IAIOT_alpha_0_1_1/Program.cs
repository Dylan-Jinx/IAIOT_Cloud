using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IAIOT_alpha_0_1_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //日志输出的配置
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
                    loggingBuilder.AddFilter("System", LogLevel.Warning);
                    loggingBuilder.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
