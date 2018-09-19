using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Ansa.GeoNames.Sqlite;
using Ansa.GeoNames.SqlServer;

namespace Ansa.GeoNames
{
    class Program
    {
        // TODO: Abstract more common code from Sqlite & SqlServer implementations
        // TODO: Add full suite of GeoNames tables
        // CONSIDER: Class library with API vs current console app environment

        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            try
            {
                Configuration = builder.Build();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error obtaining configuration data. Please check your appsettings.json file.");
                Console.Write(exception);
            }

            var database = Configuration["Database"].ToLowerInvariant();

            Console.WriteLine("Configured. Starting application...");
            Console.WriteLine("Ready to create database. Press any key to begin.");
            Console.ReadLine();

            switch (database)
            {
                case "sqlite":
                    SqliteDB.CreateDB(Configuration);
                    break;
                case "sqlserver":
                    SqlServerDB.CreateDB(Configuration);
                    break;
                default:
                    Console.WriteLine("Invalid database type specified.");
                    break;
            }
        }
    }
}
