using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Ansa.Extensions;
using NGeoNames;

namespace Ansa.GeoNames.SqlServer
{
    public static class PopulateGeoNames
    {
        public static void Populate(IConfiguration configuration)
        {
            Console.WriteLine("Getting ready to populate GeoNames...");

            var connectionString = configuration["ConnectionString"];
            var dataPath = configuration["DataSourcePath"];
            var minimumPopulation = configuration["GeoNames:CitiesMinimumPopulation"];

            string citiesFileName;

            switch (minimumPopulation)
            {
                case "1000":
                    citiesFileName = "cities1000";
                    break;
                case "5000":
                    citiesFileName = "cities5000";
                    break;
                case "15000":
                    citiesFileName = "cities15000";
                    break;
                default:
                    citiesFileName = "cities15000";
                    break;
            }

            var citiesPath = Path.Combine(dataPath, citiesFileName + ".txt");

            if (!File.Exists(citiesPath))
            {
                Console.WriteLine("Downloading GeoNames data...");
                var downloader = GeoFileDownloader.CreateGeoFileDownloader();
                downloader.DownloadFile(citiesFileName + ".zip", dataPath);
            }

            var results = GeoFileReader.ReadExtendedGeoNames(citiesPath).OrderBy(p => p.Id);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Populating GeoNames...");

                var allowIdentityInsert = connection.CreateCommand();
                allowIdentityInsert.CommandText = @"SET IDENTITY_INSERT GeoNames ON";

                try
                {
                    allowIdentityInsert.ExecuteNonQuery();
                }
                catch (SqlException exception)
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
                catch (SqlException exception)
                {
                    Console.WriteLine("SQL Exception occurred. Error Code: " + exception.ErrorCode);

                }

                Console.WriteLine();
            }
        }
    }
}
