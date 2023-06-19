using System;
using Mentore.Models;
using Mentore.Models.DAL;
using Mentore.Services.Interfaces;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs.Settings;
using Mentore.Services;
using Mentore.Services.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mentore.Services.TokenGenerators;
using Mentore.Services.TokenValidators;
using Mentore.Commons.VNPay;
using API.Services;
using API.Services.Interfaces;
using API.Model.DAL.Interfaces;
using API.Model.DAL.Repositories;
using DAL.Entities;

namespace Mentore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped((Func<IServiceProvider, Func<MentoreContext>>)((provider) => () => provider.GetService<MentoreContext>()));
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<VNPaySettings>(configuration.GetSection("VNPaySettings"));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, userRepo>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRefreshTokenRepository, refreshTokenRepo>()
                .AddScoped<IUserGroupRepository, UserGroupRepository>()
                .AddScoped<ICredentialRepository, CredentialRepository>()
                .AddScoped<IBankRepository, BankRepository>()
                .AddScoped<IMentorRepository, MentorRepository>()
                .AddScoped<IMenteeRepository, MenteeRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IMentorPositionRepository, MentorPositionRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<IWorkshopRepository, WorkshopRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<ILocationRepository, LocationRepository>()
                .AddScoped<IFieldRepository, FieldRepository>()
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<IEntityFieldRepository, EntityFieldRepository>()
                .AddScoped<IBankTypeRepository, BankTypeRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<Encryptor>()
                .AddScoped<AccessTokenGenerator>()
                .AddScoped<RefreshTokenGenerator>()
                .AddScoped<RefreshTokenValidator>()
                .AddScoped<TokenGenerator>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserGroupService, UserGroupService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IBankService, BankService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IAdminService, AdminService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IMentorService, MentorService>();
        }
    }
}