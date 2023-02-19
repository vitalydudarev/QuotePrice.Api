using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuotePrice.Api.DTOs;
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
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(QuoteDto))]
    public async Task<ActionResult<QuoteDto>> GetQuotePrice(string currencyPair, [FromQuery] string source)
    {
        var quote = await _quoteService.GetQuoteAsync(source, currencyPair);
        if (quote != null)
        {
            var quoteDto = _mapper.Map<QuoteDto>(quote);
            
            return Ok(quoteDto);
        }

        return BadRequest();
    }
    
    [HttpGet("history")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<QuoteDto>))]
    public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotePriceHistory(string currencyPair)
    {
        var quote = await _quoteService.GetQuoteAsync("source", currencyPair);
        if (quote != null)
        {
            return Ok(new QuoteDto
            {
                Ask = quote.Ask,
                Bid = quote.Bid,
                Timestamp = quote.Timestamp
            });
        }

        return BadRequest();
    }
}