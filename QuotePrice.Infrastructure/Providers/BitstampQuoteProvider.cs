using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitstampQuoteProvider : IQuoteProvider
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private readonly string _url;
    
    private readonly JsonSerializerOptions? _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    public BitstampQuoteProvider(ILogger logger, HttpClient httpClient, string url)
    {
        _logger = logger;
        _httpClient = httpClient;
        _url = url;
    }
    
    // add Automapper

    public async Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        BitstampQuoteResponse? response = null;
        try
        {
            var responseMessage = await _httpClient.GetAsync($"https://www.bitstamp.net/api/v2/ticker/{currencyPair}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<BitstampQuoteResponse>(responseString, _jsonSerializerOptions);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occurred while executing request");
            return null;
        }
        
        _logger.LogInformation("Request executed successfully");

        return response != null
            ? new Quote
            {
                Ask = ConvertDouble(response.Ask),
                Bid = ConvertDouble(response.Bid),
                High = ConvertDouble(response.High),
                Low = ConvertDouble(response.Low),
                Volume = ConvertDouble(response.Volume),
                Timestamp = ConvertDouble(response.Timestamp),
                Last = ConvertDouble(response.Last)
            }
            : null;
    }

    private static double? ConvertDouble(string? s)
    {
        return !string.IsNullOrEmpty(s) ? double.Parse(s, CultureInfo.InvariantCulture) : null;
    }
}