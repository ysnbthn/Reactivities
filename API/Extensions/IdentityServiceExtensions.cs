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
                    // signalR bağlantısı için jwt ekle
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            // istekleri atarken tokenı da ekle
                            if(!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
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