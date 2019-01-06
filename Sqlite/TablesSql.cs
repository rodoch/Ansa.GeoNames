namespace Ansa.GeoNames.Sqlite
{
    public static class TablesSql
    {
        public const string GeoNames = @"CREATE TABLE IF NOT EXISTS [GeoNames] (
                [ID] INTEGER NOT NULL PRIMARY KEY,
                [Name] NVARCHAR(200) NULL,
                [NameASCII] NVARCHAR(200) NULL,
                [Latitude] DECIMAL(18,15) NULL, 
                [Longitude] DECIMAL(18,15) NULL, 
                [FeatureClass] NCHAR(1) NULL, 
                [FeatureCode] NVARCHAR(10) NULL, 
                [CountryCode] NVARCHAR(2) NULL,
                [Population] INTEGER,
                [Elevation] INTEGER NULL,
                [Dem] INTEGER,
                [Timezone] NVARCHAR(40) NULL,
                [ModificationDate] DATE NULL
            )";

        public const string GeoNamesIndex = @"CREATE INDEX IF NOT EXISTS [IX_GeoNames] ON [GeoNames] (
                [Latitude] ASC,
                [Longitude] ASC,
                [Name] ASC,
                [NameASCII] ASC,
                [Population] ASC,
                [CountryCode] ASC
            )";

        public const string CountryInfo = @"CREATE TABLE IF NOT EXISTS [CountryInfo] (
                [ISO_Alpha2] TEXT PRIMARY KEY,
                [ISO_Alpha3] TEXT NOT NULL,
                [ISO_Numeric] TEXT NOT NULL,
                [FIPS] TEXT NOT NULL,
                [Country] TEXT NOT NULL,
                [Capital] TEXT NOT NULL,
                [Area] INT NOT NULL,
                [Population] INT NOT NULL,
                [Continent] TEXT NOT NULL,
                [Tld] TEXT NOT NULL,
                [CurrencyCode] TEXT NOT NULL,
                [CurrencyName] TEXT NOT NULL,
                [Phone] TEXT NOT NULL,
                [PostalCodeFormat] TEXT NULL,
                [PostalCodeRegex] TEXT NULL,
                [GeoNameId] INT NOT NULL,
                [EquivalentFipsCode] TEXT NOT NULL
            )";

        public const string CountryInfoIndex = @"CREATE INDEX IF NOT EXISTS [IX_CountryInfo] ON [CountryInfo] (
                [ISO_Alpha2] ASC,
                [ISO_Alpha3] ASC,
                [ISO_Numeric] ASC,
                [GeoNameId] ASC,
                [Population] ASC,
                [Country] ASC,
                [Continent] ASC
            )";

        public const string AlternateNames = @"CREATE TABLE IF NOT EXISTS [AlternateNames] (
                [ID] INTEGER NOT NULL PRIMARY KEY,
                [GeoNameId] INT NOT NULL,
                [ISOLanguage] TEXT NULL,
                [AlternateName] TEXT NULL,
                [IsPreferredName] INT NULL, 
                [IsShortName] INT NULL, 
                [IsColloquial] INT NULL, 
                [IsHistoric] INT NULL,
                [FromDate] TEXT NULL,
                [ToDate] TEXT NULL
            )";

        public const string AlternateNamesIndex = @"CREATE INDEX IF NOT EXISTS [IX_AlternateNames] ON [AlternateNames] (
                [AlternateName] ASC,
                [GeoNameId] ASC,
                [ISOLanguage] ASC,
                [IsPreferredName] ASC,
                [IsShortName] ASC,
                [IsHistoric] ASC
            )";
    }
}