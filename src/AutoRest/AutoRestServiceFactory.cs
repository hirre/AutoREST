using System.Net;
using AutoRest.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
        public static IWebHost Create(string[] args, int port)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => { options.Listen(IPAddress.Loopback, port); })
                .UseStartup<Startup>()
                .Build();
        }
    }
}