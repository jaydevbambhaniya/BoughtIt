using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;

namespace Application.Commands
{
    public record AuthUserCommand : IRequest<ApiResponse<AuthResponseDto>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
    public class AuthUserCommandHandler : IRequestHandler<AuthUserCommand, ApiResponse<AuthResponseDto>>
    {
        public IUserService userService { get; set; }
        public AuthUserCommandHandler(IUserService userRepository)
        {
            this.userService = userRepository;
        }
        public async Task<ApiResponse<AuthResponseDto>> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            return await userService.AuthenticateAsync(request.UserName, request.Password);
        }
    }
}
