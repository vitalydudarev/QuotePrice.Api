using QuotePrice.Infrastructure;

namespace QuotePrice.Tests;

public class SnakeCaseNamingPolicyTests
{
    [Fact]
    public void ShouldConvertNameCorrectly()
    {
        var policy = new SnakeCaseNamingPolicy();
        var result = policy.ConvertName("TestName");
        
        Assert.Equal("test_name", result);
    }
}