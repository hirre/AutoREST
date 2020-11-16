using System.Net;
using AutoRest.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoRest
{
    /// <summary>
    ///     Service factory for the web host.
    /// </summary>
    public static class AutoRestServiceFactory
    {
        /// <summary>
        ///     Get web host.
        /// </summary>
        /// <param name="args">Start arguments</param>
        /// <param name="port">Server listen port</param>
        /// <returns>Web host</returns>
        public static IHost Create(string[] args, int port)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(serverOptions => { serverOptions.ListenAnyIP(port); });
                })
                .Build();
        }
    }
}