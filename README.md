## Requirements
- NET 6
- Docker (for running Postres database)

## Run/create migrations:
NOTE: before creating migrations check that you local version of the Entity Framework tools is up-to-date, otherwise it may affect applying the migrations from the code. In case the Entity Framework tools are outdated run the following command:
```console
dotnet tool update --global dotnet-ef
```

Migrations must be run the root folder:
```console
dotnet ef migrations add <MigrationName> -s QuotePrice.Api --project ./Migrations/QuotePrice.Sqlite -- --DatabaseType Sqlite
dotnet ef migrations add <MigrationName> -s QuotePrice.Api --project ./Migrations/QuotePrice.Postgres -- --DatabaseType Postgres
```