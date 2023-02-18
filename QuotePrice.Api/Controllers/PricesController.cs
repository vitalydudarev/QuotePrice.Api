using Microsoft.AspNetCore.Mvc;
using QuotePrice.Api.DTOs;
using QuotePrice.Services;

namespace QuotePrice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PricesController : ControllerBase
{
    // private readonly BitfinexQuotePriceProvider _quotePriceProvider;
    private readonly BitstampQuotePriceProvider _quotePriceProvider;

    public PricesController(BitstampQuotePriceProvider quotePriceProvider)
    // public PricesController(BitfinexQuotePriceProvider quotePriceProvider)
    {
        _quotePriceProvider = quotePriceProvider;
    }
    // add logging
    //
    // should return DTOs
    /*[HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
    public ActionResult<IEnumerable<string>> Get()
    {
        return Ok(new[] { "aaa", "bbb" });
    }*/
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(QuotePriceDto))]
    public async Task<ActionResult<QuotePriceDto>> GetQuotePrice(string currencyPair)
    {
        var quotePrice = await _quotePriceProvider.GetPriceAsync(currencyPair);
        if (quotePrice != null)
        {
            return Ok(new QuotePriceDto
            {
                Ask = quotePrice.Ask,
                Bid = quotePrice.Bid,
                High = quotePrice.High,
                Low = quotePrice.Low,
                Last = quotePrice.Last,
                Volume = quotePrice.Volume,
                Timestamp = quotePrice.Timestamp
            });
        }

        return BadRequest();
    }
    
    
    
    // https://www.bitstamp.net/api/#ticker
    // https://www.bitstamp.net/api/v2/ticker/btcusd/
    /*
     * timestamp	:	1676711690
        open	:	24577
        high	:	25014
        low	:	23688
        last	:	24574
        volume	:	2897.05432716
        vwap	:	24361
        bid	:	24576
        ask	:	24578
        open_24	:	23761
        percent_change_24	:	3.42
     */
    
    // https://docs.bitfinex.com/v1/reference/rest-public-ticker#rest-public-ticker
    // https://api.bitfinex.com/v1/pubticker/btcusd
    /*
     * mid	:	24565.5
       bid	:	24565.0
       ask	:	24566.0
       last_price	:	24564.0
       low	:	23685.0
       high	:	25023.0
       volume	:	4002.70438975
       timestamp	:	1676711815.2142653
     */
}