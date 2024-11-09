using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Commands
{
    public record ExternalLoginCommand : IRequest<ApiResponse<AuthResponseDto>>
    {
        public string? Code { get; set; }
    }
    public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, ApiResponse<AuthResponseDto>>
    {
        private readonly IUserService _userService;
        public ExternalLoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ApiResponse<AuthResponseDto>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            return await this._userService.ExternalLoginAsync(request.Code);
        }
    }
}
