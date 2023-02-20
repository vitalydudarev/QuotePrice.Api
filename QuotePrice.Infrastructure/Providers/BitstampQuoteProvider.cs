using Microsoft.Extensions.DependencyInjection;
using QuotePrice.Domain.Models;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitstampQuoteProvider : QuoteProviderBase<BitstampQuoteResponse>
{
    public BitstampQuoteProvider(IServiceScopeFactory serviceScopeFactory, string url, string sourceName)
        : base(serviceScopeFactory, url, sourceName)
    {
    }

    public override Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        // currencyPair parameter is case-sensitive
        return base.GetQuoteAsync(currencyPair.ToLower());
    }
}