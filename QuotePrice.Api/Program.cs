using Microsoft.EntityFrameworkCore;
using QuotePrice.Api.Mappers;
using QuotePrice.Api.Middlewares;
using QuotePrice.Domain;
using QuotePrice.Domain.Repositories;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Database;
using QuotePrice.Infrastructure.Mappers;
using QuotePrice.Infrastructure.Repositories;
using QuotePrice.Postgres;
using QuotePrice.Services.Factories;
using QuotePrice.Services.Services;
using QuotePrice.Sqlite;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure();

app.Run();


void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddHttpClient();
    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile<ModelDtoMappingProfile>();
        config.AddProfile<ResponseModelMappingProfile>();
        config.AddProfile<ModelDbEntityMappingProfile>();
    });
    services.AddMemoryCache();
    
    var databaseType = configurationManager.GetValue<string>("DatabaseType");

    services.AddDbContext<QuoteDbContext>(opt =>
    {
        switch (databaseType)
        {
            case "InMemory":
                opt.UseInMemoryDatabase(databaseName: "Test");
                break;

            case "Sqlite":
            {
                var connectionString = configurationManager.GetConnectionString("Sqlite");
                var migrationsAssembly = typeof(SqliteMigrations).Assembly.GetName().Name;
                
                opt.UseSqlite(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(migrationsAssembly));
            }
                break;
            
            case "Postgres":
            {
                var connectionString = configurationManager.GetConnectionString("Postgres");
                var migrationsAssembly = typeof(PostgresMigrations).Assembly.GetName().Name;

                opt.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(migrationsAssembly));
            }
                break;
        }
    });
        
    /*switch (databaseType)
    {
        case "InMemory":
            services.AddDbContext<QuoteDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "Test"));
            break;

        case "Sqlite":
        {
            var connectionString = configurationManager.GetConnectionString("Sqlite");
            services.AddDbContext<QuoteDbContext>(opt =>
                opt.UseSqlite(connectionString,
                    optionsBuilder =>
                        optionsBuilder.MigrationsAssembly(typeof(SqliteMigrations).Assembly.GetName().Name)));
        }
            break;

        case "Postgres":
        {
            var connectionString = configurationManager.GetConnectionString("Postgres");
            services.AddDbContext<QuoteDbContext>(opt =>
                opt.UseNpgsql(connectionString,
                    optionsBuilder =>
                        optionsBuilder.MigrationsAssembly(typeof(PostgresMigrations).Assembly.GetName().Name)));
        }
            break;
    }*/
    
    /*"ConnectionStrings": {
        "Sqlite" : "Data Source=vehicles.db",
        "Postgres" : "User ID=postgres;Password=Pass123!;Server=localhost;Port=5432;Database=vehicles;"
    }
    
    // services.AddDbContext<QuoteDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "Test"));
    services.AddDbContext<QuoteDbContext>(opt =>
        opt.UseSqlite($"Data Source=quotes.db", optionsBuilder => optionsBuilder.MigrationsAssembly("rrrr")));
    */
    AddServices(services);
}

void AddServices(IServiceCollection services)
{
    services.AddScoped<IQuoteService, QuoteService>();
    services.AddScoped<IQuoteSourceService, QuoteSourceService>();
    services.AddScoped<IQuoteProviderFactory, QuoteProviderFactory>();
    services.AddScoped<IQuoteStoreService, QuoteStoreService>();
    services.AddScoped<IQuoteRepository, QuoteRepository>();
}

void Configure()
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}
