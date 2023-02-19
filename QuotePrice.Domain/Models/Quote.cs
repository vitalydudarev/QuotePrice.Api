namespace QuotePrice.Domain.Models;

public class Quote
{
    public string Pair { get; set; } = null!;
    public double? Timestamp { get; set; }
    public double? Bid { get; set; }
    public double? Ask { get; set; }
    public double? Low { get; set; }
    public double? High { get; set; }
    public double? Last { get; set; }
    public double? Volume { get; set; }
}
