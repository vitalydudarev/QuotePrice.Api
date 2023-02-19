namespace QuotePrice.Domain;

public interface IQuoteProviderFactory
{
    IQuoteProvider? CreateProvider(string source);
}