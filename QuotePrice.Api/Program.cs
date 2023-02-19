using QuotePrice.Domain;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Providers;
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
    
    AddServices(services);
}

void AddServices(IServiceCollection services)
{
    services.AddScoped<BitfinexQuoteProvider>();
    services.AddScoped<BitstampQuoteProvider>();
    services.AddScoped<IQuoteService, QuoteService>();
    services.AddScoped<IQuoteSourceService, QuoteSourceService>();
    services.AddScoped<IQuoteProviderFactory, QuoteProviderFactory>();
}

void Configure()
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}
