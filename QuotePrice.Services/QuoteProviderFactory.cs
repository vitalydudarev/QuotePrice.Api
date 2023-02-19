using AutoMapper;
using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services;

public class QuoteProviderFactory : IQuoteProviderFactory
{
    private readonly ILogger<QuoteProviderFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IQuoteSourceService _quoteSourceService;

    public QuoteProviderFactory(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        HttpClient httpClient,
        IQuoteSourceService quoteSourceService)
    {
        _logger = loggerFactory.CreateLogger<QuoteProviderFactory>();
        _loggerFactory = loggerFactory;
        _mapper = mapper;
        _httpClient = httpClient;
        _quoteSourceService = quoteSourceService;
    }
    
    public IQuoteProvider? CreateProvider(string source)
    {
        var quoteSources = _quoteSourceService.GetQuoteSources().ToDictionary(a => a.Name.ToLower(), b => b);
        if (!quoteSources.TryGetValue(source.ToLower(), out var quoteSource))
        {
            _logger.LogError("Provider source {Source} not found", source);
            throw new Exception($"Provider source {source} not found");
        }

        var type = Type.GetType(quoteSource.ImplementationClass);
        if (type != null)
        {
            var logger = _loggerFactory.CreateLogger(type);

            var parameters = new object[] {logger, _mapper, _httpClient, quoteSource.Url};

            try
            {
                var instance = Activator.CreateInstance(type, parameters);
                if (instance != null)
                {
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