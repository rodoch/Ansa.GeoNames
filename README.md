# Ansa.GeoNames

This ASP.NET Core 2.0 console application will populate a data store with Gazetteer data from the [GeoNames geographical database](http://www.geonames.org/). These data can already be accessed via a public API or text-file data dumps but sometimes it is useful to be able to write SQL queries to a relational database. Ansa.GeoNames wraps the excellent [NGeoNames](https://github.com/RobThree/NGeoNames) library, which handles the heavy-lifting in terms of retrieving and parsing the GeoNames data, and will generate your choice of a Sqlite or SQL Server database.

- [Status](#status)
- [Schema](#schema)
- [Installation and setup](#installation-and-setup)
- [Configuration](#configuration)
- [Building a Sqlite database](#building-a-sqlite-database)
- [Building a SQL Server database](#building-a-sql-server-database)

## Status

I wrote this application to address a need common to a number of my customers. It is not, as yet, comprehensive for all other use cases. The output data set includes tables for (1) GeoNames, (2) alternate Names, and (3) country info. There are not yet tables for feature codes, admin codes, time zones and the other reference lists included in the GeoNames database. I feel the provided tables do solve the most common needs of users, but I am interested in producing a fully-featured version of the application, particularly if other developers are interested.

## Schema

The database schema mirrors the GeoNames Gazetteer data structure described [here](http://download.geonames.org/export/dump/).

## Intallation and setup

First, clone the repository to your local machine:

```
git clone https://github.com/rodoch/Ansa.GeoNames
```

**Note:** This application has a dependency on the NGeoNames library (see above). The release-version NGeoNames package does not yet support the GeoNames AlternateNamesV2 schema, however. I have opened a PR [here](https://github.com/RobThree/NGeoNames/pull/7) to add V2 support. When this PR is accepted, or a better solution is implemented, I will reference the mainstream package in the Ansa.GeoNames project. In the meantime, you will need to clone my fork of NGeoNames [(here)](https://github.com/rodoch/NGeoNames) and include the project reference within Ansa.GeoName.csproj.

Then, build the ASP.NET solution, specifying your target runtime environment, e.g.:

```cmd
dotnet build -r win10-x64
```

**Tip:** See a list of target runtime identifiers [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog?irgwc=1&OCID=AID681541_aff_7593_1243925&tduid=(ir_6d4f9ce9N213458eb7517c20a2b9db916)(7593)(1243925)(je6NUbpObpQ-wDYfcuMFmHDb6Ja3HC_Ryw)()&irclickid=6d4f9ce9N213458eb7517c20a2b9db916#using-rids?ranMID=24542&ranEAID=je6NUbpObpQ&ranSiteID=je6NUbpObpQ-wDYfcuMFmHDb6Ja3HC_Ryw&epi=je6NUbpObpQ-wDYfcuMFmHDb6Ja3HC_Ryw).

This will produce an .exe file and an appsettings.json file that you can grab from `<PATH-TO-YOUR-APP>/bin/Debug/netcoreapp2.0/`.

## Configuration

Use the **appsettings.json** file to configure your target database:

```json
{
  "Database": "Sqlite",
  "ConnectionString": "Data Source=D:\\GeoNamesOutput\\Geonames.sqlite",
  "DataSourcePath": "D:\\GeoNamesData",
  "GeoNames": {
    "AlternateNamesLanguageCodes": "ga,en",
    "CitiesMinimumPopulation": "15000"
  }
}
```

| Key | Description | Values |
| --- | --- | --- |
| **Database** | Specifies the type of database you wish to build | `Sqlite` OR `SQLServer` |
| **ConnectionString** | DB connection string | Note that a connection string for a Sqlite DB must include the `Data Source=` prefix |
| **DataSourcePath** | Where you intend to store the raw text files from the GeoNames data store, prior to DB input. | Directory path |
| **GeoNames:AlternateNamesLanguageCodes** | Specify the languages for which you wish to have alternate toponymic names | Comma-separated string of ISO Alpha-2 language codes. An empty value means all languages will be retrieved. |
| **GeoNames:CitiesMinimumPopulation** | The minimum population a population centre must have to be included in the GeoNames dataset | `1000` OR `5000` OR `15000` |

Things to note:

- You will not need to retrieve the GeoNames raw data dumps yourself. The application will do this for you. You need to specify the DataSourcePath in order to decide where these files will be stored.
- Some GeoNames raw text files range from 0.5-1.5GB unzipped, depending on the configuration you choose. Ensure you have adequate disk space.
- Probably the biggest factor in download size, processing speed, and the size of the subsequent database is the number of alternate languages you specify.
- The next biggest facot is the minimum population settings. Higher minima mean smaller file sizes.

Unsurprisingly, the SQL Server implementation writes data to the DB significantly faster. If creating a Sqlite database you might want to make yourself a cup of tea.

## Building a Sqlite database

Ansa.GeoNames will create the database and all the necessary tables automatically. You just need to run the program with the appropriate configuration, e.g.

```json
{
  "Database": "Sqlite",
  "ConnectionString": "Data Source=D:\\GeoNamesOutput\\GeoNames.sqlite",
  "DataSourcePath": "D:\\GeoNamesData",
  "GeoNames": {
    "AlternateNamesLanguageCodes": "ga,en",
    "CitiesMinimumPopulation": "15000"
  }
}
```

Remember to:

1. Specify "Sqlite" as your `Database` value.
2. Specify a connection string, including the `Data Source=` prefix.

## Building a SQL Server database

1. Create a new database on your SQL Server instance and give it an appropriate name, e.g. GeoNames. Set the recovery model and compatibility according to your own requirements. `Latin1_General_CI_AI` collation seems to be the best choice for this type of database.
2. Run the provided SQL script in `/DBScripts/SqlServer/` to generate the tables.
3. Specify your configuration in appsettings.json, e.g.:

```json
{
  "Database": "SQLServer",
  "ConnectionString": "Server=localhost;Database=GeoNames;Trusted_Connection=True;",
  "DataSourcePath": "D:\\GeoNamesData",
  "GeoNames": {
    "AlternateNamesLanguageCodes": "ga,en",
    "CitiesMinimumPopulation": "15000"
  }
}
```

### Permissions

Please note that the database user specified in your SQL Server connection string must have ALTER permission on the database you are writing to. This is required so that IDENTITY_INSERT can be turned on for the duration of the data input process as we are inserting premade IDs in to the PK/Identity columns of the GeoNames and AlternateNames tables.