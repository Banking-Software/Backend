using System.Text;
using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MicroFinance.ServiceExtensions.IdentityService
{
    public static class SuperAdminIdentityService
    {
        public static async Task<IServiceCollection> AddSuperAdminIdentityServiceAsync(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<SuperAdmin>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<SuperAdminDbContext>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            });
            builder.AddSignInManager<SignInManager<SuperAdmin>>();
            builder.AddUserManager<UserManager<SuperAdmin>>();
            services.AddAuthentication();
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