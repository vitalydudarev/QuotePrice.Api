using QuotePrice.Domain;
using QuotePrice.Domain.Models;

namespace QuotePrice.Tests.Stubs;

public class QuoteProviderStub : IQuoteProvider
{
    public QuoteProviderStub(IServiceProvider serviceProvider, string url)
    {
    }
        
    public Task<Quote?> GetQuoteAsync(string currencyPair)
    {
        throw new NotImplementedException();
    }
}