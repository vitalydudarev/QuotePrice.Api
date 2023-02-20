using Microsoft.Extensions.DependencyInjection;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitstampQuoteProvider : QuoteProviderBase<BitstampQuoteResponse>
{
    public BitstampQuoteProvider(IServiceScopeFactory serviceScopeFactory, string url)
        : base(serviceScopeFactory, url, "Bitstamp")
    {
    }
}