using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services;

public class QuoteService : IQuoteService
{
    private readonly ILogger<QuoteService> _logger;
    private readonly IQuoteProviderFactory _quoteProviderFactory;

    public QuoteService(ILogger<QuoteService> logger, IQuoteProviderFactory quoteProviderFactory)
    {
        _logger = logger;
        _quoteProviderFactory = quoteProviderFactory;
    }
    
    public async Task<Quote?> GetQuoteAsync(string source, string currencyPair)
    {
        try
        {
            var provider = _quoteProviderFactory.CreateProvider(source);
            if (provider != null)
            {
                return await provider.GetQuoteAsync(currencyPair);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occured: {Error}", e.Message);
        }

        return null;
    }
}