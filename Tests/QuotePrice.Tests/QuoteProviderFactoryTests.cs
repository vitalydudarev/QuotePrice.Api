using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;
using QuotePrice.Services;
using QuotePrice.Services.Factories;
using QuotePrice.Tests.Stubs;

namespace QuotePrice.Tests;

public class QuoteProviderFactoryTests
{
    [Fact]
    public void ShouldCreateProvider()
    {
        var providerType = typeof(QuoteProviderStub);
        const string quoteSourceName = "Test";
        
        var mock = new Mock<IQuoteSourceService>();

        mock.Setup(a => a.GetQuoteSources()).Returns(new[]
        {
            new QuoteSource
            {
                Name = quoteSourceName,
                Url = "URL",
                ImplementationClass = $"{providerType.FullName}, {providerType.Assembly.FullName}"
            }
        });
        
        var factory = new QuoteProviderFactory(new NullLogger<QuoteProviderFactory>(), null, mock.Object, GetMemoryCache());

        var provider = factory.CreateProvider(quoteSourceName);
        
        Assert.NotNull(provider);
    }

    private static IMemoryCache GetMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider.GetService<IMemoryCache>();
    }
}