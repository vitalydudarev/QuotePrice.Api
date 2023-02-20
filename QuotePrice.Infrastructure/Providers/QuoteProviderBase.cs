using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;

namespace QuotePrice.Infrastructure.Providers;

public class QuoteProviderBase<T> : IQuoteProvider
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _url;
    private readonly string _sourceName;

    private readonly JsonSerializerOptions? _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    protected QuoteProviderBase(IServiceScopeFactory serviceScopeFactory, string url, string sourceName)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _url = url;
        _sourceName = sourceName;
    }

    public async Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<QuoteProviderBase<T>>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
        
        try
        {
            var responseMessage = await httpClient.GetAsync($"{_url}/{currencyPair}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseString, _jsonSerializerOptions);
                
                logger.LogInformation("Request executed successfully");

                return response != null
                    ? mapper.Map<Quote>(response, options => options.AfterMap((_, quote) =>
                    {
                        quote.Pair = currencyPair;
                        quote.Source = _sourceName;
                    }))
                    : null;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error has occurred while executing request");
        }
        
        return null;
    }
}