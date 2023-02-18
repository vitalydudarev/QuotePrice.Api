namespace QuotePrice.Services.Responses;

public class BitstampQuotePriceResponse
{
    public string? Timestamp { get; set; }
    public string? Bid { get; set; }
    public string? Ask { get; set; }
    public string? Last { get; set; }
    public string? Low { get; set; }
    public string? High { get; set; }
    public string? Volume { get; set; }
    public string? Vwap { get; set; }
    public string? Open { get; set; }
    public string? Open24 { get; set; }
    public string? PercentChange24 { get; set; }
}