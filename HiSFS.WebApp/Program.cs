using HiSFS.Api.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Syncfusion.DocIO.DLS;
using Syncfusion.Licensing.crypto.encodings;

namespace HiSFS.WebApp
{
    public class Program
    {
        public static readonly bool DEBUG = true;

        public static void Main(string[] args)
        {
            //MyRecord a = new (P1: "a", P2: "b");
            //var b = a with { P1 = "c" };

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    if (Program.DEBUG == true)
                    {
                        services.AddHostedService<WampApiService>();
                    }
                });
    }

    //public record MyRecord(string P1, string P2);
}