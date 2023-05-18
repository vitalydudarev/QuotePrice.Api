using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuotePrice.Api.DTOs;
using QuotePrice.Domain.Models;
using QuotePrice.Domain.Services;

namespace QuotePrice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IQuoteService _quoteService;

    public QuotesController(IMapper mapper, IQuoteService quoteService)
    {
        _mapper = mapper;
        _quoteService = quoteService;
    }
    
    /// <summary>
    /// Returns prices (bid/ask) for the requested currency pair
    /// </summary>
    /// <param name="currencyPair">Currency pair (e.g. BTC/USD)</param>
    /// <param name="source">Source of the prices</param>
    /// <returns>Quote containing bid, ask and timestamp. If failed or not found returns null or 400 Bad request</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuoteDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuoteDto>> GetQuotePrice(string currencyPair, [FromQuery] string source)
    {
        var quote = await _quoteService.GetQuoteAsync(source, currencyPair.Replace("/", ""));
        if (quote != null)
        {
            var quoteDto = _mapper.Map<QuoteDto>(quote);
            
            return Ok(quoteDto);
        }

        return BadRequest();
    }
    
    /// <summary>
    /// Returns history of loaded prices
    /// </summary>
    /// <param name="request">Parameter to filter prices by Source and/or Currency Pair</param>
    /// <returns>A list of prices</returns>
    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<QuoteDto>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<QuoteDto>))]
    public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotePriceHistory([FromQuery] QuoteHistoryRequest? request)
    {
        // TODO: add pagination
        var requestParameters = _mapper.Map<QuoteQueryParameters>(request);
        var quotes = await _quoteService.GetQuoteHistoryAsync(requestParameters);
        
        var quoteDtos = _mapper.Map<IEnumerable<QuoteDto>>(quotes);
            
        return Ok(quoteDtos);
    }
}