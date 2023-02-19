using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;

namespace QuotePrice.Tests.Stubs;

public class QuoteProviderStub : IQuoteProvider
{
    public QuoteProviderStub(ILogger logger, HttpClient httpClient, string url)
    {
    }
        
    public Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        throw new NotImplementedException();
    }
}