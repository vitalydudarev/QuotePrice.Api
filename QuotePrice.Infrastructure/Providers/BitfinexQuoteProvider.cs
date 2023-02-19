using AutoMapper;
using Microsoft.Extensions.Logging;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitfinexQuoteProvider : QuoteProviderBase<BitfinexQuoteResponse>
{
    public BitfinexQuoteProvider(ILogger logger, IMapper mapper, HttpClient httpClient, string url) 
        : base(logger, mapper, httpClient, url, "Bitfinex")
    {
    }
}