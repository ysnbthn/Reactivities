
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
// güvenlik için ekle
app.UseXContentTypeOptions();
// browser referrer info göndermesin diye
app.UseReferrerPolicy(opt => opt.NoReferrer());
// cross site scripting koruması için
app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
// başka sitede iframe içinde siteyi açmasınlar diye
app.UseXfo(opt => opt.Deny());
// güvenlik raporlarını görmek için
app.UseCsp(opt => opt
    // tüm content https olucak
    .BlockAllMixedContent()
    // kaynak içeriden geliyor demek
    .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com", "https://cdnjs.cloudflare.com", "sha256-r3x6D0yBZdyG8FpooR5ZxcsLuwuJ+pSQ/80YzwXS5IU=", "sha256-yChqzBduCCi4o4xdbXRXh4U/t1rP4UUUMJt+rB+ylUI="))
    .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "data:", "https://cdnjs.cloudflare.com"))
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self().CustomSources("https://res.cloudinary.com", "https://www.facebook.com", "data:", "https://platform-lookaside.fbsbx.com"))
    .ScriptSources(s => s.Self().CustomSources("https://connect.facebook.net", "sha256-ZgwMuaId2Y2Yr47UAwiZU35sK0zCyiEI4w3B8Vx0Tug="))
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
        await next.Invoke();
    });
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