using System.Reflection;
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
await ApplyDatabaseMigrationsAsync();

app.Run();


void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    });
    services.AddHttpClient();
    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile<ModelDtoMappingProfile>();
        config.AddProfile<ResponseModelMappingProfile>();
        config.AddProfile<ModelDbEntityMappingProfile>();
    });
    services.AddMemoryCache();
    
    ConfigureDatabase(configurationManager, services);

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

void ConfigureDatabase(ConfigurationManager configurationManager, IServiceCollection services)
{
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
            
            default:
                throw new Exception("Unsupported database provider");
        }
    });
}

async Task ApplyDatabaseMigrationsAsync()
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var context = scope.ServiceProvider.GetService<QuoteDbContext>();

    if (context != null && context.Database.IsRelational())
    {
        await context.Database.MigrateAsync();
    }
}
