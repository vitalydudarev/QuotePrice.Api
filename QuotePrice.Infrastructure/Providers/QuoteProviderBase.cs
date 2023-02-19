using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;

namespace QuotePrice.Infrastructure.Providers;

public class QuoteProviderBase<T> : IQuoteProvider
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly string _url;

    private readonly JsonSerializerOptions? _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    public QuoteProviderBase(ILogger logger, IMapper mapper, HttpClient httpClient, string url)
    {
        _logger = logger;
        _mapper = mapper;
        _httpClient = httpClient;
        _url = url;
    }

    public async Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"{_url}/{currencyPair}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseString, _jsonSerializerOptions);
                
                _logger.LogInformation("Request executed successfully");

                return response != null
                    ? _mapper.Map<Quote>(response, options => options.AfterMap((o, quote) => quote.Pair = currencyPair))
                    : null;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occurred while executing request");
        }
        
        return null;
    }
}