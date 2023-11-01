using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebSocketServer;

namespace WebSocketServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls(System.Configuration.ConfigurationManager.
                        AppSettings["ServerConnectionHTTP"],
                        System.Configuration.ConfigurationManager.
                        AppSettings["ServerConnectionHTTPS"]);
                });
    }
}
