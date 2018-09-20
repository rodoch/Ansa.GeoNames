USE [YOUR_DATABASE_NAME]

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'GeoNames')

BEGIN
	CREATE TABLE [dbo].[GeoNames] (
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](200) NULL,
		[NameASCII] [nvarchar](200) NULL,
		[Latitude] [float] NULL,
		[Longitude] [float] NULL,
		[FeatureClass] [nchar](1) NULL,
		[FeatureCode] [nvarchar](10) NULL,
		[CountryCode] [nvarchar](2) NULL,
		[Population] [int] NULL,
		[Elevation] [int] NULL,
		[Dem] [int] NULL,
		[Timezone] [nvarchar](40) NULL,
		[ModificationDate] [datetime] NULL,
	 CONSTRAINT [PK_GeoNames] PRIMARY KEY CLUSTERED ([ID] ASC)
	 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'CountryInfo')

BEGIN
	CREATE TABLE [dbo].[CountryInfo] (
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[ISO_Alpha2] [nvarchar](max) NOT NULL,
		[ISO_Alpha3] [nvarchar](max) NOT NULL,
		[ISO_Numeric] [nvarchar](max) NOT NULL,
		[FIPS] [nvarchar](max) NOT NULL,
		[Country] [nvarchar](max) NOT NULL,
		[Capital] [nvarchar](max) NOT NULL,
		[Area] [int] NOT NULL,
		[Population] [int] NOT NULL,
		[Continent] [nvarchar](max) NOT NULL,
		[Tld] [nvarchar](max) NOT NULL,
		[CurrencyCode] [nvarchar](max) NOT NULL,
		[CurrencyName] [nvarchar](max) NOT NULL,
		[Phone] [nvarchar](max) NOT NULL,
		[PostalCodeFormat] [nvarchar](max) NULL,
		[PostalCodeRegex] [nvarchar](max) NULL,
		[GeoNameId] [int] NOT NULL,
		[EquivalentFipsCode] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_CountryInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
	 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'AlternateNames')

BEGIN
	CREATE TABLE [dbo].[AlternateNames] (
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[GeoNameId] [int] NOT NULL,
		[ISOLanguage] [nvarchar](max) NULL,
		[AlternateName] [nvarchar](max) NULL,
		[IsPreferredName] [bit] NOT NULL,
		[IsShortName] [bit] NOT NULL,
		[IsColloquial] [bit] NOT NULL,
		[IsHistoric] [bit] NOT NULL,
		[FromDate] [nvarchar](max) NULL,
		[ToDate] [nvarchar](max) NULL,
	 CONSTRAINT [PK_AlternateNames] PRIMARY KEY CLUSTERED ([ID] ASC)
	 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END