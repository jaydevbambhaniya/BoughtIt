using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        public Task<(User,bool)> AuthenticateAsync(string username, string password);
        public Task<ApiResponse<object>> CreateUserAsync(User user);
        public Task<User?> GetUserDetailsAsync(int? userId=null,string? email=null);
        public Task<int> UpdateUserDetailsAsync(UserDto user);
        public Task<int> UpdateUserPasswordAsync(int UserId,string OldPassword,string NewPassword);
        public Task<bool> UpdateUserRefreshTokensAsync(int UserId,string refreshToken);
    }
}
