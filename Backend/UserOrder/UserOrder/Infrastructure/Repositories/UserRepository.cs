using Domain.Model;
using Domain.Repository;
using FluentValidation;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly BoughtItDbContext _boughtItDbContext;
        public UserRepository(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            BoughtItDbContext boughtItDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _boughtItDbContext = boughtItDbContext;
        }
        public async Task<(User?,bool)> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return (user,false);
            }
            var result = await _signInManager.PasswordSignInAsync(user, password,false,false);
            return (user,result.Succeeded);
        }

        public async Task<ApiResponse<object>> CreateUserAsync(User user)
        {
            string role = "";
            if (user.Email.Contains("@boughtit.com"))
            {
                role = Role.Admin.ToString();
            }
            else role = Role.User.ToString();

            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
            user.Role = role;
            IdentityResult result=null;
            if (user.IsExternalLogin==true)
            {
                result = await _userManager.CreateAsync(user);
            }
            else
            {
                result = await _userManager.CreateAsync(user, user.PasswordHash);
            }
            if (!result.Succeeded)
            {
                return new ApiResponse<object>() { StatusCode = result.Succeeded ? 1 : -1, Message = result.Succeeded ? "" : result.Errors.FirstOrDefault().Description };
            }
            result = await _userManager.AddToRoleAsync(user, role);
            return new ApiResponse<object>() { StatusCode = result.Succeeded ? user.Id : -1, Message = result.Succeeded?"": result.Errors.FirstOrDefault().Description };
        }

        public async Task<User?> GetUserDetailsAsync(int? userId=null,string? email=null)
        {
            if (userId.HasValue)
            {
                return await _userManager.FindByIdAsync(userId.ToString());
            }
            if (!string.IsNullOrEmpty(email))
            {
                return await _userManager.FindByEmailAsync(email);
            }

            return null;
        }

        public async Task<int> UpdateUserDetailsAsync(UserDto user)
        {
            var data = await _userManager.FindByIdAsync(user.Id.ToString());
            data.FirstName = user.FirstName;
            data.LastName = user.LastName;
            data.Email = user.Email;
            data.PhoneNumber = user.PhoneNumber;
            data.Gender = user.Gender;
            var result = await _userManager.UpdateAsync(data);
            return result.Succeeded ? 1 : -1;
        }
        public async Task<bool> UpdateUserRefreshTokensAsync(int userId,string refreshToken)
        {
            var user = _boughtItDbContext.Users.Where(user=>user.Id == userId).FirstOrDefault();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(1);
            int n = await _boughtItDbContext.SaveChangesAsync();
            return n > 0;
        }
        public async Task<int> UpdateUserPasswordAsync(int UserId, string OldPassword, string NewPassword)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null) return -1;
            bool isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, OldPassword);
            if (!isOldPasswordCorrect)
            {
                return -10;
            }
            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            Console.WriteLine(result.Errors.FirstOrDefault());
            return result.Succeeded? 1 : -1;
        }
    }
}
