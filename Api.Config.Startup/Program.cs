using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Api.Config.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder();
            try
            {
                host.UseKestrel();
                host.UseUrls();
                host.UseContentRoot(Directory.GetCurrentDirectory());
                host.UseIISIntegration();
                host.UseStartup<Startup>();
                var webhost = host.Build();

                webhost.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
