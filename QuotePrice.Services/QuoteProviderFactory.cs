using Microsoft.Extensions.Logging;
using QuotePrice.Domain;
using QuotePrice.Domain.Services;

namespace QuotePrice.Services;

public class QuoteProviderFactory : IQuoteProviderFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly HttpClient _httpClient;
    private readonly IQuoteSourceService _quoteSourceService;

    public QuoteProviderFactory(ILoggerFactory loggerFactory, HttpClient httpClient, IQuoteSourceService quoteSourceService)
    {
        _loggerFactory = loggerFactory;
        _httpClient = httpClient;
        _quoteSourceService = quoteSourceService;
    }
    
    public IQuoteProvider? CreateProvider(string source)
    {
        var quoteSources = _quoteSourceService.GetQuoteSources().ToDictionary(a => a.Name, b => b);
        var quoteSource = quoteSources[source];

        var type = Type.GetType(quoteSource.ImplementationClass);
        if (type != null)
        {
            var logger = _loggerFactory.CreateLogger(type);

            var parameters = new object[] {logger, _httpClient, quoteSource.Url};

            var instance = Activator.CreateInstance(type, parameters);
            if (instance != null)
            {
                return (IQuoteProvider)instance;
            }
        }

        return null;
    }
}