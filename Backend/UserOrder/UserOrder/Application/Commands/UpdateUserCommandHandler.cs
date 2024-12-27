using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Commands
{
    public record UpdateUserCommand :IRequest<ApiResponse<int>>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse<int>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserDetails(_mapper.Map<UserDto>(request));
        }
    }
}
