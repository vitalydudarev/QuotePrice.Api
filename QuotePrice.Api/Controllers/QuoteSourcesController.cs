using Microsoft.AspNetCore.Mvc;

namespace QuotePrice.Api.Controllers;

[ApiController]
[Route("api/quote-sources")]
public class QuoteSourcesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
    public ActionResult<IEnumerable<string>> Get()
    {
        return Ok(new[] { "Bitfinex", "Bitstamp" });
    }
}