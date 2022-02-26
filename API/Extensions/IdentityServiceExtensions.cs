using System.Text;
using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config){
                
                services.AddIdentityCore<AppUser>(opt=>{
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager<SignInManager<AppUser>>();
                
                // bunun yerine keyi direkt enviorement variable olarak gömebilirsin
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
                
                // authenticationa jwt ekle
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt=>{
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
                // edit activity için auth policy ekle
                services.AddAuthorization(opt=>
                {
                    opt.AddPolicy("IsActivityHost", policy =>
                    {
                        policy.Requirements.Add(new IsHostRequirement());
                    });
                });
                // sadece metod çalışırken lazım ondan transient
                services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
                // token servisini ekle
                services.AddScoped<TokenService>();

                return services;
        }
    }
}