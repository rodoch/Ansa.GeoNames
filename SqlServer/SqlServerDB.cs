using System;
using Microsoft.Extensions.Configuration;

namespace Ansa.GeoNames.SqlServer
{
    public class SqlServerDB
    {
        public static void CreateDB(IConfiguration configuration)
        {
            Console.WriteLine("Creating SQL Server database...");

            // NOTE: the DB user must have ALTER permission on the database you are writing to.
            // This is so that IDENTITY_INSERT can be temporarily turned on.
            // This is necessary so that IDs can be inserted into the PK/Identity columns of the GeoNames and AlternateNames tables.

            PopulateCountries.Populate(configuration);
            PopulateCountryInfo.Populate(configuration);
            PopulateGeoNames.Populate(configuration);
            PopulateAlternateNames.Populate(configuration);

            Console.WriteLine("Finished!");
        }
    }
}
