using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace AutoRest.RunService
{
    public static class Program
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
            AutoRestServiceFactory.Create(args, port).Run();

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