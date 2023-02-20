using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services.Factories;

public class QuoteProviderFactory : IQuoteProviderFactory
{
    private readonly ILogger<QuoteProviderFactory> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IQuoteSourceService _quoteSourceService;
    private readonly IMemoryCache _memoryCache;

    public QuoteProviderFactory(
        ILogger<QuoteProviderFactory> logger,
        IServiceScopeFactory serviceScopeFactory,
        IQuoteSourceService quoteSourceService,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _quoteSourceService = quoteSourceService;
        _memoryCache = memoryCache;
    }
    
    public IQuoteProvider? CreateProvider(string source)
    {
        var quoteSources = _quoteSourceService.GetQuoteSources().ToDictionary(a => a.Name.ToLower(), b => b);
        if (!quoteSources.TryGetValue(source.ToLower(), out var quoteSource))
        {
            _logger.LogError("Provider source {Source} not found", source);
            throw new Exception($"Provider source {source} not found");
        }

        var cachedProvider = _memoryCache.Get<IQuoteProvider>(quoteSource.ImplementationClass);
        if (cachedProvider != null)
        {
            return cachedProvider;
        }

        var type = Type.GetType(quoteSource.ImplementationClass);
        if (type != null)
        {
            var parameters = new object[] {_serviceScopeFactory,  quoteSource.Url, quoteSource.Name};

            try
            {
                var instance = Activator.CreateInstance(type, parameters);
                if (instance != null)
                {
                    // TODO: all instances should be disposed at a later stage - implement it
                    _memoryCache.Set(quoteSource.ImplementationClass, instance);
                    return (IQuoteProvider)instance;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to create provider of {Type}", quoteSource.ImplementationClass);
                throw new Exception($"Unable to create provider of {quoteSource.ImplementationClass}");
            }
        }

        return null;
    }
}