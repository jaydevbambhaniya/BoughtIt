using Domain.Repository;
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
    public record CreateUserCommand : IRequest<ApiResponse<object>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<object>>
    { 
        private IUserService userService;
        public CreateUserCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<ApiResponse<object>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await userService.CreateUserAsync(request);
        }
    }
}
