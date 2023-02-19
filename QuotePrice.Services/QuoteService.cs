using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services;

public class QuoteService : IQuoteService
{
    private readonly ILogger<QuoteService> _logger;
    private readonly IQuoteProviderFactory _quoteProviderFactory;
    private readonly IQuoteStoreService _quoteStoreService;

    public QuoteService(
        ILogger<QuoteService> logger,
        IQuoteProviderFactory quoteProviderFactory,
        IQuoteStoreService quoteStoreService)
    {
        _logger = logger;
        _quoteProviderFactory = quoteProviderFactory;
        _quoteStoreService = quoteStoreService;
    }
    
    public async Task<Quote?> GetQuoteAsync(string source, string currencyPair)
    {
        try
        {
            var provider = _quoteProviderFactory.CreateProvider(source);
            if (provider != null)
            {
                var quote = await provider.GetQuoteAsync(currencyPair);
                if (quote != null)
                {
                    await _quoteStoreService.SaveAsync(quote);
                }

                return quote;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occured: {Error}", e.Message);
        }

        return null;
    }
    
    public async Task<IEnumerable<Quote>> GetQuoteHistoryAsync(string source, string currencyPair)
    {
        try
        {
            var quotes = await _quoteStoreService.GetAllAsync();

            return quotes;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occured: {Error}", e.Message);
        }

        return Array.Empty<Quote>();
    }
}