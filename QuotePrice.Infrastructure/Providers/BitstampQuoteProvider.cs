using AutoMapper;
using Microsoft.Extensions.Logging;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Providers;

public class BitstampQuoteProvider : QuoteProviderBase<BitstampQuoteResponse>
{
    public BitstampQuoteProvider(ILogger logger, IMapper mapper, HttpClient httpClient, string url)
        : base(logger, mapper, httpClient, url, "Bitstamp")
    {
    }
}