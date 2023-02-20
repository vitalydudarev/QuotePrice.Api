namespace QuotePrice.Api.DTOs;

public class QuoteHistoryRequest
{
    public string? CurrencyPair { get; set; }
    public string? Source { get; set; }
}