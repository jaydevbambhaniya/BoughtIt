using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Commands;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ApiResponse<AuthResponseDto>> AuthenticateAsync(string username, string password);
        public Task<string> GenerateJWTTokenAsync(User user,string role);
        public Task<ApiResponse<object>> CreateUserAsync(CreateUserCommand command);
        public Task<ApiResponse<UserDto>> GetUserDetailsAsync(int userId);
        public Task<ApiResponse<int>> UpdateUserDetails(UserDto userDto);
        public Task<ApiResponse<int>> UpdateUserPassword(string oldPassword, string newPassword, int UserId);
        public Task<ApiResponse<TokenDto>> RefreshTokenAsync(TokenDto token);
        public Task<ApiResponse<AuthResponseDto>> ExternalLoginAsync(string code);

    }
}
