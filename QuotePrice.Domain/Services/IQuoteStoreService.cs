using QuotePrice.Domain.Models;

namespace QuotePrice.Domain.Services;

public interface IQuoteStoreService
{
    Task SaveAsync(Quote quote);
    Task<IEnumerable<Quote>> GetAllAsync();
}