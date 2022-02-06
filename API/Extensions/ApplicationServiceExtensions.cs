using Application.Activities;
using Application.Core;
using MediatR;
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

            // Add services to the container
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // React'ın API'da olan cors policy'i geçmesi için
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });
            // mediator ekle ve yerini göster
            builder.Services.AddMediatR(typeof(List.Handler).Assembly);
            // add automapper
            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        }

    }
}