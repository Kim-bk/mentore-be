using Mentore.Models;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using System.Threading.Tasks;
using Mentore.Services.TokenGenerators;
using System.IdentityModel.Tokens.Jwt;
using System;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mentore.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUserRepository _userRepo;

        public AuthService(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator
                , IRefreshTokenRepository refreshTokenRepo, IUnitOfWork unitOfWork
                , IMapperCustom mapper, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepo = refreshTokenRepo;
            _userRepo = userRepo;
        }

        public async Task<TokenResponse> Authenticate(Account user, string listCredentials, string userGroup = "")
        {
            try
            {
                // 1. Generate access vs refresh token
               var accessToken = _accessTokenGenerator.Generate(user, listCredentials);
                var refreshToken = _refreshTokenGenerator.Generate();

                // 2. Init refresh token properties
                string refreshTokenId = Guid.NewGuid().ToString();
                string refreshTokenHandler = new JwtSecurityTokenHandler().WriteToken(refreshToken);

                // 3. Create user refresh token
                RefreshToken userRefreshToken = new()
                {
                    Id = refreshTokenId,
                    UserId = user.Id,
                    Token = refreshTokenHandler,
                };

                await _refreshTokenRepo.AddAsync(userRefreshToken);
                await _unitOfWork.CommitTransaction();

                // 5. Return two tokens (AccessToken vs RefreshToken vs ShopId vs Wallet)

                return new TokenResponse()
                {
                    IsSuccess = true,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshTokenHandler,
                    UserId = user.Id
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();

                return new TokenResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}