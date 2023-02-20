using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Repositories;
using QuotePrice.Infrastructure.Database;
using QuotePrice.Infrastructure.Database.Entities;

namespace QuotePrice.Infrastructure.Repositories;

public class QuoteRepository : IQuoteRepository
{
    private readonly ILogger<QuoteRepository> _logger;
    private readonly QuoteDbContext _context;
    private readonly IMapper _mapper;

    public QuoteRepository(ILogger<QuoteRepository> logger, QuoteDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    
    public async Task SaveAsync(Quote quote)
    {
        try
        {
            var dbQuote = _mapper.Map<DbQuote>(quote);
            await _context.Quotes.AddAsync(dbQuote);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occured while adding new entity: {Error}", e.Message);
            throw new Exception("An error has occured while adding new entity");
        }
    }

    public async Task<IEnumerable<Quote>> GetAllAsync(QuoteQueryParameters? parameters)
    {
        try
        {
            var queryable = _context.Quotes.AsQueryable().AsNoTracking();
            queryable = HandleQueryParameters(queryable, parameters);
            
            var dbQuotes = await queryable.ToListAsync();

            return _mapper.Map<IEnumerable<Quote>>(dbQuotes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occured while retrieving entities: {Error}", e.Message);
            throw new Exception("An error has occured while retrieving entities");
        }
    }

    private static IQueryable<DbQuote> HandleQueryParameters(IQueryable<DbQuote> queryable, QuoteQueryParameters? parameters)
    {
        if (parameters != null)
        {
            if (parameters.CurrencyPair != null)
            {
                queryable = queryable.Where(a => EF.Functions.Like(a.Source, $"%{parameters.CurrencyPair}%"));
            }

            if (parameters.Source != null)
            {
                queryable = queryable.Where(a => EF.Functions.Like(a.Source, $"%{parameters.Source}%"));
            }
        }

        return queryable;
    }
}