
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Activities;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
// wwwroot klasöründeki dosyaları görsün
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("CorsPolicy");
//jwt authentication servisini ekle
app.UseAuthentication();

app.UseAuthorization();

//apiları çağırırken kullanıyorsun
app.MapControllers();
// signalR ekle
app.MapHub<ChatHub>("/chat");
// static dosyalar için yönlendirme
//app.MapFallbackToController("Index","Fallback");
app.MapFallbackToFile("index.html");
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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context, userManager);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }
    }
}