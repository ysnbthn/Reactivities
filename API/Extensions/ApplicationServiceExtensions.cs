using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {

        // Service işlemleri için WebApplicationBuilder builder kullan
        public static void AddServices(WebApplicationBuilder builder)
        {
            //Db context
            var connectionString = builder.Configuration.GetConnectionString("conn");
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite(connectionString));

            // Add services to the container then fluent validation
            builder.Services.AddControllers(opt =>
            {
                // tüm endpointlere authorization ekliyorsun
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<Create>();
                });



            builder.Services.AddIdentityServices(builder.Configuration);



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // React'ın API'da olan cors policy'i geçmesi için
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
                });
            });
            // mediator ekle ve yerini göster
            builder.Services.AddMediatR(typeof(List.Handler).Assembly);
            // add automapper
            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            // interface ile implementasyonunu DI Containera ekle
            builder.Services.AddScoped<IUserAccessor, UserAccessor>();
            builder.Services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            builder.Services.AddSignalR();

            // add cloudinary
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

        }

    }
}