using Microsoft.Extensions.Logging;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Providers;

namespace QuotePrice.Services;



public class QuoteService : IQuoteService
{
    public async Task<Quote?> GetQuoteAsync(string source, string currencyPair)
    {
        await Task.Delay(100);

        return null;
    }
}