using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [Authorize]
    [Route("/api/product")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("getProduct")]
        //[Authorize(policy:"CombinePolicy")]
        [AllowAnonymous]
        [ResponseCache(Location =ResponseCacheLocation.Any,Duration =120)]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete("deleteProduct")]
        [Authorize(policy: "AdminPolicy")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("addProduct")]
        //[Authorize(policy: "AdminPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProduct([FromForm] AddProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("updateProduct")]
        [Authorize(policy: "AdminPolicy")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("getAllProducts")]
        [Authorize(policy: "CombinePolicy")]
        [ResponseCache(Location =ResponseCacheLocation.Any,Duration =120)]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            return Ok(await _mediator.Send(query));
        }
        [HttpGet("getFilteredProducts")]
        [Authorize(policy: "CombinePolicy")]
        public async Task<IActionResult> GetSearchProducts(GetSearchProductsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
