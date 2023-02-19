using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Providers;

namespace QuotePrice.Services;

public class QuoteSourceService : IQuoteSourceService
{
    public IEnumerable<QuoteSource> GetQuoteSources()
    {
        // ideally this should be moved to a database table and then fetched from there
        return new[]
        {
            new QuoteSource
            {
                Name = "Bitfinex",
                Url = "https://api.bitfinex.com/v1/pubticker/",
                ImplementationClass = nameof(BitfinexQuoteProvider)
            },
            new QuoteSource
            {
                Name = "Bitstamp",
                Url = "https://www.bitstamp.net/api/v2/ticker/",
                ImplementationClass = nameof(BitstampQuoteProvider)
            }
        };
    }
}