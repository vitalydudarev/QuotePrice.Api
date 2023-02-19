using QuotePrice.Domain.Models;

namespace QuotePrice.Domain.Services;

public interface IQuoteService
{
    Task<Quote?> GetQuoteAsync(string source, string currencyPair);
}