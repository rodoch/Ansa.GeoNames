namespace Ansa.GeoNames.Sqlite
{
    public static class TablesSql
    {
        public const string Geonames = @"CREATE TABLE IF NOT EXISTS [Geonames] (
                ID INTEGER NOT NULL PRIMARY KEY, 
                Name NVARCHAR(200) NULL, 
                NameASCII NVARCHAR(200) NULL,
                Latitude DECIMAL(18,15) NULL, 
                Longitude DECIMAL(18,15) NULL, 
                FeatureClass NCHAR(1) NULL, 
                FeatureCode NVARCHAR(10) NULL, 
                CountryCode NVARCHAR(2) NULL,
                Population INTEGER, 
                Elevation INTEGER NULL, 
                Dem INTEGER,
                Timezone NVARCHAR(40) NULL, 
                ModificationDate DATE NULL
            )";

        public const string Countries = @"CREATE TABLE IF NOT EXISTS [CountryInfo] (
                ISO_Alpha2 TEXT PRIMARY KEY,
                ISO_Alpha3 TEXT NOT NULL,
                ISO_Numeric TEXT NOT NULL,
                FIPS TEXT NOT NULL,
                Country TEXT NOT NULL,
                Capital TEXT NOT NULL,
                Area INT NOT NULL,
                Population INT NOT NULL,
                Continent TEXT NOT NULL,
                Tld TEXT NOT NULL,
                CurrencyCode TEXT NOT NULL,
                CurrencyName TEXT NOT NULL,
                Phone TEXT NOT NULL,
                PostalCodeFormat TEXT NULL,
                PostalCodeRegex TEXT NULL,
                GeoNameId INT NOT NULL,
                EquivalentFipsCode TEXT NOT NULL
            )";

        public const string AlternateNames = @"CREATE TABLE IF NOT EXISTS [AlternateNames] (
                ID INTEGER NOT NULL PRIMARY KEY, 
                GeoNameId INT NOT NULL,
                ISOLanguage TEXT NULL, 
                AlternateName TEXT NULL,
                IsPreferredName INT NULL, 
                IsShortName INT NULL, 
                IsColloquial INT NULL, 
                IsHistoric INT NULL,
                FromDate TEXT NULL,
                ToDate TEXT NULL
            )";
    }
}