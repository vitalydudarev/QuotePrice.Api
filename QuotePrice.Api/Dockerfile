FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuotePrice.Api/QuotePrice.Api.csproj", "QuotePrice.Api/"]
COPY ["Migrations/QuotePrice.Postgres/QuotePrice.Postgres.csproj", "Migrations/QuotePrice.Postgres/"]
COPY ["QuotePrice.Infrastructure/QuotePrice.Infrastructure.csproj", "QuotePrice.Infrastructure/"]
COPY ["QuotePrice.Domain/QuotePrice.Domain.csproj", "QuotePrice.Domain/"]
COPY ["Migrations/QuotePrice.Sqlite/QuotePrice.Sqlite.csproj", "Migrations/QuotePrice.Sqlite/"]
COPY ["QuotePrice.Services/QuotePrice.Services.csproj", "QuotePrice.Services/"]
RUN dotnet restore "QuotePrice.Api/QuotePrice.Api.csproj"
COPY . .
WORKDIR "/src/QuotePrice.Api"
RUN dotnet build "QuotePrice.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuotePrice.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuotePrice.Api.dll"]
