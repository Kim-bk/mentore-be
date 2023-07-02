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
                .AddScoped<IMentorRepository, MentorRepository>()
                .AddScoped<IMenteeRepository, MenteeRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IWorkshopRepository, WorkshopRepository>()
                .AddScoped<ILocationRepository, LocationRepository>()
                .AddScoped<IFieldRepository, FieldRepository>()
                .AddScoped<IEntityFieldRepository, EntityFieldRepository>()
                .AddScoped<IAppointmentRepository, AppointmentRepository>()
                .AddScoped<IExperienceRepository, ExperienceRepository>()
                .AddScoped<ISpeakerWorkshopRepository, SpeakerWorkshopRepository>()
                .AddScoped<IUserWorkshopRepository, UserWorkshopRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<Encryptor>()
                .AddScoped<AccessTokenGenerator>()
                .AddScoped<UploadImageService>()
                .AddScoped<RefreshTokenGenerator>()
                .AddScoped<RefreshTokenValidator>()
                .AddScoped<TokenGenerator>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserGroupService, UserGroupService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IAdminService, AdminService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IMentorService, MentorService>()
                .AddScoped<IWorkshopService, WorkshopService>()
                .AddScoped<IAppointmentService, AppointmentService>();
        }
    }
}