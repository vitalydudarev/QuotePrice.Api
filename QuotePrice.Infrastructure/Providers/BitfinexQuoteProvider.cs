using Microsoft.Extensions.DependencyInjection;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitfinexQuoteProvider : QuoteProviderBase<BitfinexQuoteResponse>
{
    public BitfinexQuoteProvider(IServiceScopeFactory serviceScopeFactory, string url, string sourceName) 
        : base(serviceScopeFactory, url, sourceName)
    {
    }
}