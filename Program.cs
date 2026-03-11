using GestãoDeIdeasV2.Data;
using GestãoDeIdeasV2.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt",
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddDbContext<IdeaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IdeasService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddHttpClient<IAdviceService, AdviceService>(client =>
{
    client.BaseAddress = new Uri("https://api.adviceslip.com/advice");
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ideas.API v2");
        c.DocumentTitle = "Ideas.API";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IdeaContext>();
    db.Database.Migrate();
    IdeaMaintenanceService.UpdateOutdatedIdeas(db, DateTime.UtcNow);
}

app.Run();
