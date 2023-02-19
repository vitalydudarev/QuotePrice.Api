using Microsoft.EntityFrameworkCore;
using QuotePrice.Api.Mappers;
using QuotePrice.Api.Middlewares;
using QuotePrice.Domain;
using QuotePrice.Domain.Repositories;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Database;
using QuotePrice.Infrastructure.Mappers;
using QuotePrice.Infrastructure.Repositories;
using QuotePrice.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

Configure();

app.Run();


void ConfigureServices(IServiceCollection services)
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
    services.AddDbContext<QuoteDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "Test"));
    
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
