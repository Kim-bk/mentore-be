using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL;
using Mentore.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Mentore.Models.DTOs.Responses;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs;
using Mentore.Services.Interfaces;
using Mentore.Services.TokenValidators;

namespace Mentore.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        public RefreshTokenGenerator(IRefreshTokenRepository refreshTokenRepo, TokenGenerator tokenGenerator
                        , IConfiguration configuration, IUnitOfWork unitOfWork, RefreshTokenValidator refreshTokenValidator
                        , IUserRepository userRepo)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepo = refreshTokenRepo;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _userRepo = userRepo;
            _refreshTokenValidator = refreshTokenValidator;
        }
        public async Task<UserResponse> Refresh(string tokenContent)
        {
            await _unitOfWork.BeginTransaction();
            // 1. Check if refresh token is valid
            var validRefreshToken = _refreshTokenValidator.Validate(tokenContent);

            if (!validRefreshToken.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = validRefreshToken.ErrorMessage
                };
            }

            // 2. Get refresh token by token
            var rs = await GetByToken(tokenContent);

            if (!rs.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = rs.ErrorMessage
                };
            }

            var refreshTokenDTO = rs.RefreshToken;

            // 3. Delete that refresh token
            var deleteRefreshToken = await Delete(refreshTokenDTO.Id);
            if (!deleteRefreshToken.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = deleteRefreshToken.ErrorMessage
                };
            }
            // 4. Find user have that refresh token
            var user = refreshTokenDTO.User;
            await _unitOfWork.CommitTransaction();
            
            return new UserResponse
            {
                IsSuccess = true,
                User = user
            };
        }

        public JwtSecurityToken Generate()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:RefreshTokenSecret"]));
            var issuer = _configuration["AuthSettings:Issuer"];
            var audience = _configuration["AuthSettings:Audience"];
            var expires = DateTime.UtcNow.AddMonths(4); // expires in 4 months later

            return _tokenGenerator.GenerateToken(key, issuer, audience, expires);
        }
        private async Task<RefreshTokenResponse> GetByToken(string token)
        {
            var refreshToken = await _refreshTokenRepo.FindAsync(tk => tk.Token == token);

            if (refreshToken == null)
            {
                return new RefreshTokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Refresh Token không tìm thấy trong cơ sở dữ liệu !",
                };
            }

            return new RefreshTokenResponse
            {
                IsSuccess = true,
                RefreshToken = refreshToken
            };
        }
        private async Task<RefreshTokenResponse> Delete(string tokenId)
        {
            try
            {
                await _refreshTokenRepo.Delete(tk => tk.Id == tokenId);
                return new RefreshTokenResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new RefreshTokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}
