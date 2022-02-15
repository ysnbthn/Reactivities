
using API.Extensions;
using API.Middleware;
using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// tüm servisler static classa taşındı
ApplicationServiceExtensions.AddServices(builder);

var app = builder.Build();

// kendi yaptığın middleware'i ekle
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthorization();

//apiları çağırırken kullanıyorsun
app.MapControllers();

// database fonksiyonu
await autoMigrate(app);

app.Run();


// database fonksiyonları için Webapplication app
async Task autoMigrate(WebApplication app)
{
    // Datase yoksa otomatik migration yapıp database yapıcak
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            //üstte datacontexti service olarak containera eklediğimiz için kullanabiliyoruz
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }
    }
}