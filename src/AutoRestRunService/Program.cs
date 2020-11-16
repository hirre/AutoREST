using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AutoRest.RunService
{
    public static class Program
    {
        private const int DEFAULT_PORT = 5000;

        /// <summary>
        ///     The main method.
        /// </summary>
        /// <param name="args">Start arguments</param>
        public static async Task Main(string[] args)
        {
            var port = GetPort(args);

            // Create the service and block until closed.
            using var host = AutoRestServiceFactory.Create(args, port);

            await host.RunAsync();
            await host.StopAsync();

            Console.WriteLine("Server stopped.");
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
                    if (!args[i].Equals("--port", StringComparison.Ordinal)) continue;

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