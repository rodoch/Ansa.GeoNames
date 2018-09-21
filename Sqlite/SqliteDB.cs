using Microsoft.Extensions.Configuration;
using System;

namespace Ansa.GeoNames.Sqlite
{
    public static class SqliteDB
    {
        public static void CreateDB(IConfiguration configuration)
        {
            Console.WriteLine("Creating Sqlite database...");

            CreateTables.Create(configuration);
            //PopulateAllCountries.Populate(configuration);
            PopulateCountryInfo.Populate(configuration);
            PopulateGeoNames.Populate(configuration);
            PopulateAlternateNames.Populate(configuration);

            Console.WriteLine("Finished!");
        }
    }
}
