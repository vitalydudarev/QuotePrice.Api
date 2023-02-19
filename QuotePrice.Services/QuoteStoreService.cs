using Microsoft.Extensions.Logging;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Repositories;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services;

public class QuoteStoreService : IQuoteStoreService
{
    private readonly ILogger<QuoteStoreService> _logger;
    private readonly IQuoteRepository _quoteRepository;

    public QuoteStoreService(ILogger<QuoteStoreService> logger, IQuoteRepository quoteRepository)
    {
        _logger = logger;
        _quoteRepository = quoteRepository;
    }
    
    public async Task SaveAsync(Quote quote)
    {
        try
        {
            await _quoteRepository.SaveAsync(quote);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to save entity");
            throw new Exception("Unable to save entity");
        }
    }

    public Task<IEnumerable<Quote>> GetAllAsync()
    {
        try
        {
            return _quoteRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to retrieve entities");
            throw new Exception("Unable to retrieve entities");
        }
    }
}