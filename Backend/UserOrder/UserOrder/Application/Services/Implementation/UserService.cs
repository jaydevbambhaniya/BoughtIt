using AutoMapper;
using Domain.Model;
using Domain.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserOrder.Application.Commands;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Resources;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;

namespace UserOrder.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public UserService(IConfiguration configuration,IUserRepository userRepository,IMapper mapper,HttpClient httpClient)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<AuthResponseDto>> AuthenticateAsync(string username, string password)
        {
            var (user,isSuccessful) = await _userRepository.AuthenticateAsync(username, password);
            if (user == null)
            {
                var error = ErrorCodes.GetError("USER_NOT_FOUND");
                return new ApiResponse<AuthResponseDto>() { StatusCode = error.Code, Message = error.Message };
            }
            else if (!isSuccessful)
            {
                var error = ErrorCodes.GetError("INVALID_CREDENTIALS");
                return new ApiResponse<AuthResponseDto>() { StatusCode = error.Code, Message = error.Message };
            }
            var token = await GetUserTokens(user);
            var response= new AuthResponseDto() { UserID=user.Id, Tokens= token };
            await _userRepository.UpdateUserRefreshTokensAsync(user.Id, token.RefreshToken);
            return new ApiResponse<AuthResponseDto>() {StatusCode=200,Data=response};
        }
        private Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Task.FromResult(Convert.ToBase64String(randomNumber));
            }
        }
        public async Task<ApiResponse<object>> CreateUserAsync(CreateUserCommand command)
        {
            User user = _mapper.Map<User>(command);
            var response = await _userRepository.CreateUserAsync(user);
            return response;
        }

        public Task<string> GenerateJWTTokenAsync(User user,string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var SecToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                                _configuration["Jwt:Issuer"],
                                                claims,
                                                expires: DateTime.Now.AddMinutes(5),
                                                signingCredentials: Credentials);
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(SecToken));
        }

        public async Task<ApiResponse<UserDto>> GetUserDetailsAsync(int userId)
        {
            var user =  await _userRepository.GetUserDetailsAsync(userId);
            if (user == null)
            {
                var error = ErrorCodes.GetError("USER_NOT_FOUND");
                return new ApiResponse<UserDto>() { StatusCode = error.Code, Message = error.Message };
            }
            return new ApiResponse<UserDto>() { Data = _mapper.Map<UserDto>(user) };
        }

        public async Task<ApiResponse<int>> UpdateUserDetails(UserDto userDto)
        {
            var response = await _userRepository.UpdateUserDetailsAsync(userDto);
            return new ApiResponse<int>() { StatusCode=response};
        }

        public async Task<ApiResponse<int>> UpdateUserPassword(string oldPassword, string newPassword, int UserId)
        {
            var response = await _userRepository.UpdateUserPasswordAsync(UserId,oldPassword, newPassword);
            return new ApiResponse<int>() {Data=response};
        }

        public async Task<ApiResponse<TokenDto>> RefreshTokenAsync(TokenDto token)
        {
            var response = new ApiResponse<TokenDto>();
            var principal = await GetPrincipalFromExpiredToken(token.AccessToken);
            if(principal == null)
            {
                var error = ErrorCodes.GetError("INVALID_AUTH_TOKENS");
                response.StatusCode = error.Code;
                response.Message = error.Message;
                return response;
            }
            var userId = principal.FindFirst("UserId")?.Value;
            var user = await _userRepository.GetUserDetailsAsync(Convert.ToInt32(userId));
            if (user == null || !await ValidateRefreshTokenAsync(user, token.RefreshToken))
            {
                var error = ErrorCodes.GetError("INVALID_REFRESH_TOKEN");
                response.StatusCode = error.Code;
                response.Message = error.Message;
                return response;
            }

            var newTokens = new TokenDto();
            newTokens.AccessToken = await GenerateJWTTokenAsync(user, user.Role);
            newTokens.RefreshToken = token.RefreshToken;
            response.Data = newTokens;
            return response;
        }
        private async Task<bool> ValidateRefreshTokenAsync(User user,string refreshToken)
        {
            return await Task.FromResult(user.RefreshToken==refreshToken && user.RefreshTokenExpiry>DateTime.Now);
        }
        private async Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
                ValidateLifetime = false // Since its expired, don't validate
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
            if (!principal.IsValid)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal.ClaimsIdentity;
        }
        private async Task<TokenDto> GetUserTokens(User? user)
        {
            if (user == null) return null;
            var refreshToken = await GenerateRefreshTokenAsync();
            var accessToken = await GenerateJWTTokenAsync(user, user.Role);
            return new TokenDto() { AccessToken = accessToken, RefreshToken = refreshToken };
        }
        public async Task<ApiResponse<AuthResponseDto>> ExternalLoginAsync(ExternalLoginCommand userData)
        {
            var userInfo = _mapper.Map<GoogleUserInfo>(userData);
            User? user = await this._userRepository.GetUserDetailsAsync(null,userData.Email);
            if (user == null)
            {
                //New User
                var data = await _userRepository.CreateUserAsync(_mapper.Map<User>(userInfo));

                if (data.StatusCode <= 0)
                {
                    return new ApiResponse<AuthResponseDto>() {StatusCode = data.StatusCode, Message = data.Message };
                }
                user = await this._userRepository.GetUserDetailsAsync(null, userData.Email);
            }
            var tokens = await GetUserTokens(user);
            var response = new AuthResponseDto() { Tokens = tokens, UserID = user.Id };
            await _userRepository.UpdateUserRefreshTokensAsync(user.Id, tokens.RefreshToken);
            return new ApiResponse<AuthResponseDto>() { Data = response, StatusCode = 200 };
        }
    }
}
