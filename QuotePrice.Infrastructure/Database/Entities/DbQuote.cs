namespace QuotePrice.Infrastructure.Database.Entities;

public class DbQuote
{
    public long Id { get; set; }
    public string Pair { get; set; } = null!;
    public string Source { get; set; } = null!;
    public double? Timestamp { get; set; }
    public double? Bid { get; set; }
    public double? Ask { get; set; }
}