using QuotePrice.Domain.Models;

namespace QuotePrice.Domain.Repositories;

public interface IQuoteRepository
{
    Task SaveAsync(Quote quote);
    Task<IEnumerable<Quote>> GetAllAsync(QuoteQueryParameters? parameters);
}