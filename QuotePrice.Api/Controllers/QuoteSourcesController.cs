using Microsoft.AspNetCore.Mvc;
using QuotePrice.Domain.Services;

namespace QuotePrice.Api.Controllers;

[ApiController]
[Route("api/quote-sources")]
public class QuoteSourcesController : ControllerBase
{
    private readonly IQuoteSourceService _quoteSourceService;

    public QuoteSourcesController(IQuoteSourceService quoteSourceService)
    {
        _quoteSourceService = quoteSourceService;
    }
    
    /// <summary>
    /// Return a list of available price sources
    /// </summary>
    /// <returns>A list of available and supported price sources/providers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    public ActionResult<IEnumerable<string>> Get()
    {
        var sources = _quoteSourceService.GetQuoteSources().Select(a => a.Name);
        
        return Ok(sources);
    }
}