using System;
using System.Collections.Generic;
using System.Net;
using AutoRest.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AutoRest
{
    /// <summary>
    ///     Service factory for the web host.
    /// </summary>
    public static class WebServiceFactory
    {
        private const int DEFAULT_PORT = 5000;

        /// <summary>
        ///     The main method.
        /// </summary>
        /// <param name="args">Start arguments</param>
        public static void Main(string[] args)
        {
            var port = GetPort(args);

            Console.WriteLine($"Server started on port {port}.");

            // Create the service and block until closed.
            Create(args, port).Run();

            Console.WriteLine("Server stopped.");
        }

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

        /// <summary>
        ///     Get server listen port.
        /// </summary>
        /// <param name="args">Start arguments</param>
        /// <returns>Server listen port</returns>
        private static int GetPort(IReadOnlyList<string> args)
        {
            try
            {
                for (var i = 0; i < args.Count; i++)
                {
                    if (!args[i].Equals("-p", StringComparison.Ordinal)) continue;

                    if (int.TryParse(args[i + 1], out var port))
                        return port;

                    break;
                }
            }
            catch
            {
                // ignored
            }

            return DEFAULT_PORT;
        }
    }
}