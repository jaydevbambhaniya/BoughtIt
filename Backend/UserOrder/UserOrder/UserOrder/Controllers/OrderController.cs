using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserOrder.Application.Commands;
using UserOrder.Application.Queries;
using UserOrder.Domain.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserOrder.API.Controllers
{
    [Route("/api/order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getOrder")]
        [Authorize(policy: "UserPolicy")]
        public async Task<IActionResult> GetOrderById([FromQuery] GetOrderQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet]
        [Route("getUserOrders")]
        [Authorize(policy: "UserPolicy")]
        public async Task<IActionResult> GetAllOrdersByUser([FromQuery] GetAllOrdersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpPost]
        [Route("placeOrder")]
        [Authorize(policy: "UserPolicy")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            Console.WriteLine(command);
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete]
        [Route("deleteOrder")]
        [Authorize(policy: "UserPolicy")]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            Console.WriteLine(command.OrderID);
            return Ok(await _mediator.Send(command));
        }
        [HttpPut]
        [Route("updateOrder")]
        [Authorize(Policy = "CombinePolicy")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
