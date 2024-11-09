using AutoMapper;
using Domain.Model;
using Domain.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                return new ApiResponse<AuthResponseDto>() { StatusCode = -1, Message = "Unable to find user with given username" };
            }
            else if (!isSuccessful)
            {
                return new ApiResponse<AuthResponseDto>() { StatusCode = -2, Message = "Username or password is incorrect." };
            }
            var refreshToken = await GenerateRefreshTokenAsync();
            var accessToken = await GenerateJWTTokenAsync(user, user.Role);
            var response= new AuthResponseDto() { UserID=user.Id, Tokens= new TokenDto() { AccessToken=accessToken,RefreshToken=refreshToken} };
            await _userRepository.UpdateUserRefreshTokensAsync(user.Id, refreshToken);
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
                return new ApiResponse<UserDto>() { StatusCode = -1, Message = "No User Found!" };
            }
            return new ApiResponse<UserDto>() { Data = _mapper.Map<UserDto>(user) };
        }

        public async Task<ApiResponse<int>> UpdateUserDetails(UserDto userDto)
        {
            var response = await _userRepository.UpdateUserDetailsAsync(userDto);
            return new ApiResponse<int>() { Data=response};
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
                response.StatusCode = -100;
                response.Message = "Invalid Access token or refresh token";
                return response;
            }
            var userId = principal.FindFirst("UserId")?.Value;
            var user = await _userRepository.GetUserDetailsAsync(Convert.ToInt32(userId));
            if (user == null || !await ValidateRefreshTokenAsync(user, token.RefreshToken))
            {
                response.StatusCode = -100;
                response.Message = "Invalid Refresh token";
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

        public async Task<ApiResponse<AuthResponseDto>> ExternalLoginAsync(string code)
        {
            var response = await ExchangeCodeForTokensAsync(code);
            Console.WriteLine(response);
            return null;
        }
        private async Task<GoogleTokenResponse?> ExchangeCodeForTokensAsync(string code)
        {
            var tokenEndpoint = "https://oauth2.googleapis.com/token";

            var clientId = _configuration["Authentication:Google:ClientId"];
            var clientSecret = _configuration["Authentication:Google:ClientSecret"];
            var redirectUri = _configuration["Authentication:Google:RedirectUri"];

            Console.WriteLine($"code:{code} uri:{redirectUri}");

            var requestData = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

            var requestContent = new FormUrlEncodedContent(requestData);

            var response = await _httpClient.PostAsync(tokenEndpoint, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleTokenResponse>(responseContent);
        }
    }
}
