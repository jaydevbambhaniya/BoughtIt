using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserOrder.Application.Commands;
using UserOrder.Application.Queries;
using UserOrder.Application.Responses;

namespace UserOrderAPI.Controllers
{
    [Route("/api/user/")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [Authorize(policy: "CombinePolicy")]
        [HttpGet("getUserDetails")]
        public async Task<IActionResult> GetUserDetails([FromQuery] GetUserQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [Authorize(policy:"CombinePolicy")]
        [HttpPut("updateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [Authorize(policy:"CombinePolicy")]
        [HttpPut("updateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("googleLogin")]
        public async Task<IActionResult> GoogleResponse(ExternalLoginCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
