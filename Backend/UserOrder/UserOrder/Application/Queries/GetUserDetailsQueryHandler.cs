using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Queries
{
    public record GetUserQuery:IRequest<ApiResponse<UserDto>> { 
        public int UserID { get; set; }
    }
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserQuery, ApiResponse<UserDto>>
    {
        private readonly IUserService _userService;
        public GetUserDetailsQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApiResponse<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserDetailsAsync(request.UserID);
        }
    }
}
