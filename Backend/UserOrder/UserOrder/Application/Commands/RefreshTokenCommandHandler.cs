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
    public record RefreshTokenCommand : IRequest<ApiResponse<TokenDto>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<TokenDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public RefreshTokenCommandHandler(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RefreshTokenAsync(_mapper.Map<TokenDto>(request));
        }
    }
}
