using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NGeoNames;

namespace Ansa.GeoNames.SqlServer
{
    public static class PopulateCountryInfo
    {
        public static void Populate(IConfiguration configuration)
        {
            Console.WriteLine("Getting ready to populate country info...");

            var connectionString = configuration["ConnectionString"];
            var dataPath = configuration["DataSourcePath"];
            var countriesPath = Path.Combine(dataPath, @"countryInfo.txt");

            if (!File.Exists(countriesPath))
            {
                Console.WriteLine("Downloading country info...");
                var downloader = GeoFileDownloader.CreateGeoFileDownloader();
                downloader.DownloadFile("countryInfo.txt", dataPath);
            }

            var results = GeoFileReader.ReadCountryInfo(countriesPath).OrderBy(p => p.GeoNameId);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Populating country info...");

                const string sql = @"INSERT INTO CountryInfo VALUES (@ISO_Alpha2, @ISO_Alpha3, @ISO_Numeric, @FIPS, @Country, 
                    @Capital, @Area, @Population, @Continent, @Tld, @CurrencyCode, @CurrencyName, @Phone, @PostalCodeFormat, 
                    @PostalCodeRegex, @GeoNameId, @EquivalentFipsCode)";

                var command = connection.CreateCommand();
                command.CommandText = sql;

                string[] parameterNames = new[]
                {
                    "@ISO_Alpha2",
                    "@ISO_Alpha3",
                    "@ISO_Numeric",
                    "@FIPS",
                    "@Country",
                    "@Capital",
                    "@Area",
                    "@Population",
                    "@Continent",
                    "@Tld",
                    "@CurrencyCode",
                    "@CurrencyName",
                    "@Phone",
                    "@PostalCodeFormat",
                    "@PostalCodeRegex",
                    "@GeoNameId",
                    "@EquivalentFipsCode"
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
                    parameters[0].Value = r.ISO_Alpha2;
                    parameters[1].Value = r.ISO_Alpha3;
                    parameters[2].Value = r.ISO_Numeric;
                    parameters[3].Value = r.FIPS;
                    parameters[4].Value = r.Country;
                    parameters[5].Value = r.Capital;
                    parameters[6].Value = r.Area;
                    parameters[7].Value = r.Population;
                    parameters[8].Value = r.Continent;
                    parameters[9].Value = r.Tld;
                    parameters[10].Value = r.CurrencyCode;
                    parameters[11].Value = r.CurrencyName;
                    parameters[12].Value = r.Phone;
                    parameters[13].Value = r.PostalCodeFormat;
                    parameters[14].Value = r.PostalCodeRegex;
                    parameters[15].Value = r.GeoNameId;
                    parameters[16].Value = r.EquivalentFipsCode;

                    Console.WriteLine("Country: " + r.Country);

                    command.ExecuteNonQuery();
                }

                Console.WriteLine();
            }
        }
    }
}
