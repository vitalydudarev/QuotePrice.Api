using System.Globalization;
using System.Text.Json;
using QuotePrice.Services.Responses;

namespace QuotePrice.Services;

public class BitstampQuotePriceProvider
{
    private readonly HttpClient _httpClient;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
    };

    public BitstampQuotePriceProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // add logging!
    // check for 400 Bad Request error
    // add Automapper
    // add try-catch for HTTP request

    public async Task<Domain.Models.QuotePrice?> GetPriceAsync(string currencyPair)
    {
        var responseMessage = await _httpClient.GetAsync($"https://www.bitstamp.net/api/v2/ticker/{currencyPair}");
        var responseString = await responseMessage.Content.ReadAsStringAsync();
        // var response = JsonSerializer.Deserialize<BitfinexQuotePriceResponse>(responseString);
        var response = JsonSerializer.Deserialize<BitstampQuotePriceResponse>(responseString, _jsonSerializerOptions);
        // var response = https://api.bitfinex.com/v1/pubticker/{symbol}

        return response != null
            ? new Domain.Models.QuotePrice
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