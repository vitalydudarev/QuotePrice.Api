using QuotePrice.Domain.Models;

namespace QuotePrice.Domain.Services;

public interface IQuoteSourceService
{
    IEnumerable<QuoteSource> GetQuoteSources();
}