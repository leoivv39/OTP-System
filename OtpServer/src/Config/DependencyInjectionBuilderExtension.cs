using OtpServer.Facade;
using OtpServer.Mapper;
using OtpServer.Repository.Context;
using OtpServer.Repository.Model;
using OtpServer.Repository;
using OtpServer.Service;
using Microsoft.EntityFrameworkCore;
using OtpServer.Mapper.Hash;
using OtpServer.Otp;
using OtpServer.Security.Jwt;
using Microsoft.AspNetCore.Authorization;
using OtpServer.Encryption;
using OtpServer.Security.Requirement.Handler;

namespace OtpServer.Config
{
    public static class DependencyInjectionBuilderExtension
    {
        public static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            AddDbContext<UsersDbContext>(builder, "DefaultConnection");
            AddDbContext<OtpItemDbContext>(builder, "DefaultConnection");
            builder.Services.AddScoped<IDbContext<User>, UsersDbContext>();
            builder.Services.AddScoped<IDbContext<OtpItem>, OtpItemDbContext>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOtpItemRepository, OtpItemRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOtpItemService, OtpItemService>();
            builder.Services.AddScoped<IUserMapper, UserMapper>();
            builder.Services.AddScoped<IOtpItemMapper, OtpItemMapper>();
            builder.Services.AddScoped<IUserFacade, UserFacade>();
            builder.Services.AddScoped<IOtpFacade, OtpFacade>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            builder.Services.AddScoped<IOtpContext, ConfigurationOtpContext>();
            builder.Services.AddScoped<IOtpProvider, OtpProvider>();
            builder.Services.AddScoped<IEncryptionContext, ConfigEncryptionContext>();
            builder.Services.AddScoped<IEncryptionHandler, AesEncryptionHandler>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IAuthorizationHandler, TokenTypeHandler>();
            builder.Services.AddSingleton(builder.Configuration);
        }

        private static void AddDbContext<TContext>(WebApplicationBuilder builder, string connectionStringName) where TContext : DbContext
        {
            builder.Services.AddDbContext<TContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(connectionStringName)));
        }
    }
}
