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
    public record UpdateUserPasswordCommand : IRequest<ApiResponse<int>>
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }

    }
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, ApiResponse<int>>
    {
        private readonly IUserService _userService;
        public UpdateUserPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public Task<ApiResponse<int>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            return _userService.UpdateUserPassword(request.OldPassword,request.Password, request.UserId);
        }
    }
}
