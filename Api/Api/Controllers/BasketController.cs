using Api.Dtos;
using Api.Interfaces;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost("addToBasket/{personId}/{presentId}")]
    public async Task<ActionResult> AddToBasket(int personId, int presentId)
    {
        await _basketService.AddToBasket(personId, presentId);
        return Ok(new { message = "Successfully added", id = presentId });
    }

    [HttpPost("removeFromBasket/{personId}/{presentId}")]
    public async Task<ActionResult> RemoveFromBasket(int personId, int presentId)
    {
        await _basketService.RemoveFromBasket(personId, presentId);
        return Ok();
    }

    [HttpGet("getMyBasket/{personId}")]
    public async Task<ActionResult<BasketDTOs>> GetMyBasket(int personId)
    {
        var basket = await _basketService.GetMyBasket(personId);
        if (basket == null)
            return Ok(new BasketDTOs { Presents = new List<BasketItem>() });

        return Ok(basket);
    }

    [HttpDelete("clearItemCompletely/{personId}/{presentId}")]
    public async Task<ActionResult> ClearItemCompletely(int personId, int presentId)
    {
        await _basketService.ClearItemCompletely(personId, presentId);
        return Ok(new { message = "Item removed successfully" });
    }
}