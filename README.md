# QuotePrice.Api

## Run migrations:
Migrations must be run the root folder:
```console
dotnet ef migrations add InitialMigration -s QuotePrice.Api --project ./Migrations/QuotePrice.Sqlite -- --DatabaseType Sqlite
dotnet ef migrations add InitialMigration -s QuotePrice.Api --project ./Migrations/QuotePrice.Postgres -- --DatabaseType Postgres
```