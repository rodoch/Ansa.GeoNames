using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Ansa.GeoNames.Sqlite
{
    public static class CreateTables
    {
        public static void Create(IConfiguration configuration)
        {
            Console.WriteLine("Creating tables…");

            var connectionString = configuration["ConnectionString"];

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var geoNames = connection.CreateCommand();
                geoNames.CommandText = TablesSql.GeoNames;
                var geoNamesIndex = connection.CreateCommand();
                geoNamesIndex.CommandText = TablesSql.GeoNamesIndex;

                var countryInfo = connection.CreateCommand();
                countryInfo.CommandText = TablesSql.CountryInfo;
                var countryInfoIndex = connection.CreateCommand();
                countryInfoIndex.CommandText = TablesSql.CountryInfoIndex;

                var alternateNames = connection.CreateCommand();
                alternateNames.CommandText = TablesSql.AlternateNames;
                var alternateNamesIndex = connection.CreateCommand();
                alternateNamesIndex.CommandText = TablesSql.AlternateNamesIndex;

                geoNames.ExecuteNonQuery();
                geoNamesIndex.ExecuteNonQuery();
                Console.WriteLine("Created GeoNames table");

                countryInfo.ExecuteNonQuery();
                countryInfoIndex.ExecuteNonQuery();
                Console.WriteLine("Created CountryInfo table");

                alternateNames.ExecuteNonQuery();
                alternateNamesIndex.ExecuteNonQuery();
                Console.WriteLine("Created AlternateNames table");

                Console.WriteLine();
            }
        }
    }
}
