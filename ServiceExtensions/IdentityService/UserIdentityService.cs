using System.Text;
using MicroFinance.DBContext;
// using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MicroFinance.ServiceExtensions.IdentityService
{
    public static class UserIdentityService
    {
        public static async Task<IServiceCollection> AddUserIdentityServiceAsync(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<User>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddRoles<IdentityRole>();
            //builder.AddEntityFrameworkStores<UserDbContext>();
            builder.AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            });
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddUserManager<UserManager<User>>();
            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("UserToken", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        // Check if the key is same or not using same algorithm
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(config["Token:Key"])),
                        // check the issuer 
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });
            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("SuperAdminToken", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        // Check if the key is same or not using same algorithm
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(config["Token:Key"])),
                        // check the issuer 
                        ValidIssuer = config["Token:SuperIssuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}