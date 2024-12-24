using MediatR;
using Microsoft.AspNetCore.Mvc;
using VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket;
using VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.CreateBasket;
using VOEConsulting.Flame.BasketContext.Application.Baskets.Queries.GetBasket;

namespace VOEConsulting.Flame.BasketContext.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(ISender sender, ILogger<BasketController> logger) : BaseController(logger)
    {
        private readonly ISender _sender = sender;

        // GET api/basket/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasket(Guid id)
        {
            var result = await _sender.Send(new GetBasketQuery(id));
            return result.IsSuccess ? Ok(result) : HandleError(result.Error);
        }

        // POST api/basket
        [HttpPost]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketCommand command)
        {
            var result = await _sender.Send(command);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetBasket), new { id = result.Value }, result.Value);
            return HandleError(result.Error);
        }

        // POST api/basket/{id}/items
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItemToBasket(Guid id, [FromBody] AddItemToBasketCommand command)
        {
            if (id != command.BasketId)
                return BadRequest("Basket ID in URL does not match the command.");

           var result = await _sender.Send(command);
            if(result.IsSuccess)
            return NoContent();
            else
                return HandleError(result.Error);
        }

        [HttpGet("/test")]
        public string Test()
        {
            return "hello";
        }
    }

}
