## Requirements
- NET 6
- Docker (for running Postres database)

## Setup
The app provides the following store/database types:
- InMemory (default)
- Sqlite
- Postgres

The database type can be changed in appSettings.json file in QuotePrice.Api project under "DatabaseType" section.<br />
In order to switch to Postgres provider, docker-compose-postgres.yml file located in QuotePrice.Infrastructure project must be executed to run a Docker container. To do that run the following command :
```console
docker-compose -f docker-compose-postgres.yml up -d
```
To stop the container run the command:
```console
docker stop postgres_database
```

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