using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MasterServer.Sources
{
    public class ServerConfigData
    {
        public int port { get; set; }
        public string dbHost { get; set; }
        public int dbPort { get; set; }
    }

    public static class ServerConfig
    {
        private static ServerConfigData data;

        private static string configPath = "ServerConfig.json";

        public static int    Port   => data.port;
        public static string dbHost => data.dbHost;
        public static int    dbPort => data.dbPort;

        public static void Load()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(configPath, optional: false, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                data = new ServerConfigData();

                configuration.GetSection("ServerConfig").Bind(data);

                ConsoleClr.WriteLine($"-> Server config loaded", ConsoleColor.DarkGreen);
            }
            catch(Exception ex)
            {
                ConsoleClr.WriteLine($"Server config file not loaded: {ex}", ConsoleColor.DarkRed);
                Console.ReadLine();
            }
        }
    }
}
