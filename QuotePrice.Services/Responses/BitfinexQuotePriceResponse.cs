namespace QuotePrice.Services.Responses;

public class BitfinexQuotePriceResponse
{
    public string? Timestamp { get; set; }
    public string? Mid { get; set; }
    public string? Bid { get; set; }
    public string? Ask { get; set; }
    public string? LastPrice { get; set; }
    public string? Low { get; set; }
    public string? High { get; set; }
    public string? Volume { get; set; }
}