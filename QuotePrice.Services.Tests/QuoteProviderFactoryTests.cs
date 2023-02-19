using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using QuotePrice.Domain;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;
using QuotePrice.Infrastructure.Providers;
using QuotePrice.Services.Tests.Stubs;

namespace QuotePrice.Services.Tests;

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
        
        var factory = new QuoteProviderFactory(new NullLoggerFactory(), null, new HttpClient(), mock.Object);

        var provider = factory.CreateProvider(quoteSourceName);
        
        Assert.NotNull(provider);
    }
}