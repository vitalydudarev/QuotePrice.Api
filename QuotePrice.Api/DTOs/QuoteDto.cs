namespace QuotePrice.Api.DTOs;

public class QuoteDto
{
    public double? Timestamp { get; set; }
    public double? Bid { get; set; }
    public double? Ask { get; set; }
}