
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

AddServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//apiları çağırırken kullanıyorsun
app.MapControllers();

// database fonksiyonu
autoMigrate(app);

app.Run();


// Service işlemleri için WebApplicationBuilder builder kullan
void AddServices(WebApplicationBuilder builder)
{
    //Db context
    var connectionString = builder.Configuration.GetConnectionString("conn");
    builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite(connectionString));

    // Add services to the container
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

// database fonksiyonları için Webapplication app
void autoMigrate(WebApplication app){
    // Datase yoksa otomatik migration yapıp database yapıcak
    using(var scope = app.Services.CreateScope()){
        try
        {
            //üstte datacontexti service olarak containera eklediğimiz için kullanabiliyoruz
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.Migrate();

        }
        catch(Exception ex)
        {
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
        }
    }
}