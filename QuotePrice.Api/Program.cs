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
    services.AddScoped<BitfinexQuotePriceProvider>();
    services.AddScoped<BitstampQuotePriceProvider>();
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
