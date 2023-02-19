using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitfinexQuoteProvider : IQuoteProvider
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private readonly string _url;

    private readonly JsonSerializerOptions? _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    public BitfinexQuoteProvider(ILogger logger, HttpClient httpClient, string url)
    {
        _logger = logger;
        _httpClient = httpClient;
        _url = url;
    }
    
    // add Automapper

    public async Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        BitfinexQuoteResponse? response = null;

        var aa = nameof(BitfinexQuoteProvider);
        
        try
        {
            // TODO: move the URL to settings/database
            var responseMessage = await _httpClient.GetAsync($"https://api.bitfinex.com/v1/pubticker/{currencyPair}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<BitfinexQuoteResponse>(responseString, _jsonSerializerOptions);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occurred while executing request");
            return null;
        }
        
        _logger.LogInformation("Request executed successfully");
        
        // TODO: move this conversion to Automapper configuration
        return response != null
            ? new Quote
            {
                Ask = ConvertDouble(response.Ask),
                Bid = ConvertDouble(response.Bid),
                High = ConvertDouble(response.High),
                Low = ConvertDouble(response.Low),
                Volume = ConvertDouble(response.Volume),
                Timestamp = ConvertDouble(response.Timestamp),
                Last = ConvertDouble(response.LastPrice)
            }
            : null;
    }

    private static double? ConvertDouble(string? s)
    {
        return !string.IsNullOrEmpty(s) ? double.Parse(s, CultureInfo.InvariantCulture) : null;
    }
}