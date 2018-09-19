using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Ansa.GeoNames.Sqlite
{
    public static class CreateTables
    {
        public static void Create(IConfiguration configuration)
        {
            Console.WriteLine("Creating tables...");

            var connectionString = configuration["ConnectionString"];

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var geonames = connection.CreateCommand();
                geonames.CommandText = TablesSql.Geonames;

                var countries = connection.CreateCommand();
                countries.CommandText = TablesSql.Countries;

                var alternateNames = connection.CreateCommand();
                alternateNames.CommandText = TablesSql.AlternateNames;

                geonames.ExecuteNonQuery();
                Console.WriteLine("Created Geonames table");

                countries.ExecuteNonQuery();
                Console.WriteLine("Created CountryInfo table");

                alternateNames.ExecuteNonQuery();
                Console.WriteLine("Created AlternateNames table");

                Console.WriteLine();
            }
        }
    }
}
