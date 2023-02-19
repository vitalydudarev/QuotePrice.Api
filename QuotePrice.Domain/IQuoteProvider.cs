using QuotePrice.Domain.Models;

namespace QuotePrice.Domain;

public interface IQuoteProvider
{
    Task<Quote?> GetQuoteAsync(string currencyPair);
}