using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserOrder.Application.Commands;
using UserOrder.Application.Queries;

namespace UserOrder.API.Controllers
{
    [Route("/api/cart/")]
    public class CartController : Controller
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(policy: "CombinePolicy")]
        [HttpGet("getUserCart")]
        public async Task<IActionResult> GetCartProducts(GetCartProductsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [AllowAnonymous]
        [HttpPost("updateUserCart")]
        public async Task<IActionResult> UpdateCartAsync([FromBody] UpdateCartCommand command)
        {
            return Ok(await _mediator.Send(command));   
        }
        [Authorize(policy: "CombinePolicy")]
        [HttpGet("getUserWishlist")]
        public async Task<IActionResult> GetUserWishlist(GetWishlistProductsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpPost("updateUserWishlist")]
        public async Task<IActionResult> UpdateUserWishlist([FromBody] UpdateWishlistCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
