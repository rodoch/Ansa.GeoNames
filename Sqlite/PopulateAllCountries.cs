using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Ansa.Extensions;
using NGeoNames;

namespace Ansa.GeoNames.Sqlite
{
    public static class PopulateAllCountries
    {
        public static void Populate(IConfiguration configuration)
        {
            Console.WriteLine("Getting ready to populate countries...");

            var connectionString = configuration["ConnectionString"];
            var dataPath = configuration["DataSourcePath"];
            var countriesPath = Path.Combine(dataPath, @"allCountries.txt");

            if (!File.Exists(countriesPath))
            {
                Console.WriteLine("Downloading countries data...");
                var downloader = GeoFileDownloader.CreateGeoFileDownloader();
                downloader.DownloadFile("allCountries.zip", dataPath);
            }

            var results = GeoFileReader.ReadExtendedGeoNames(countriesPath).OrderBy(p => p.Id);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Populating countries...");

                var allowIdentityInsert = connection.CreateCommand();
                allowIdentityInsert.CommandText = @"SET IDENTITY_INSERT GeoNames ON";

                try
                {
                    allowIdentityInsert.ExecuteNonQuery();
                }
                catch (SqliteException exception)
                {
                    Console.WriteLine("SQL Exception occurred. Error Code: " + exception.ErrorCode);

                }

                const string sql = @"INSERT INTO GeoNames (ID, Name, NameASCII, Latitude, Longitude, FeatureClass, FeatureCode, CountryCode, 
                        Population, Elevation, Dem, Timezone, ModificationDate) 
                    VALUES (@ID, @Name, @NameASCII, @Latitude, @Longitude, @FeatureClass, @FeatureCode, @CountryCode, @Population, @Elevation,
                        @Dem, @Timezone, @ModificationDate)";

                var command = connection.CreateCommand();
                command.CommandText = sql;

                string[] parameterNames = new[]
                {
                    "@ID",
                    "@Name",
                    "@NameASCII",
                    "@Latitude",
                    "@Longitude",
                    "@FeatureClass",
                    "@FeatureCode",
                    "@CountryCode",
                    "@Population",
                    "@Elevation",
                    "@Dem",
                    "@Timezone",
                    "@ModificationDate"
                };

                DbParameter[] parameters = parameterNames.Select(pn =>
                {
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = pn;
                    command.Parameters.Add(parameter);
                    return parameter;
                })
                .ToArray();

                foreach (var r in results)
                {
                    parameters[0].Value = r.Id;
                    parameters[1].Value = r.Name.HasValueOrDBNull();
                    parameters[2].Value = r.NameASCII.HasValueOrDBNull();
                    parameters[3].Value = r.Latitude;
                    parameters[4].Value = r.Longitude;
                    parameters[5].Value = r.FeatureClass.HasValueOrDBNull();
                    parameters[6].Value = r.FeatureCode.HasValueOrDBNull();
                    parameters[7].Value = r.CountryCode.HasValueOrDBNull();
                    parameters[8].Value = r.Population;
                    parameters[9].Value = r.Elevation.HasValueOrDBNull();
                    parameters[10].Value = r.Dem;
                    parameters[11].Value = r.Timezone.HasValueOrDBNull();
                    parameters[12].Value = r.ModificationDate;

                    command.ExecuteNonQuery();

                    Console.WriteLine("GeoName ID: " + r.Id);
                }

                var disallowIdentityInsert = connection.CreateCommand();
                disallowIdentityInsert.CommandText = @"SET IDENTITY_INSERT GeoNames OFF";

                try
                {
                    disallowIdentityInsert.ExecuteNonQuery();
                }
                catch (SqliteException exception)
                {
                    Console.WriteLine("SQL Exception occurred. Error Code: " + exception.ErrorCode);

                }

                Console.WriteLine();
            }
        }
    }
}
