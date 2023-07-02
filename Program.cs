using Mentore;
using Mentore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:8080"
                              , "http://192.168.1.9:8080"
                              , "http://localhost:8000/"
                              , "http://localhost:41783")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                      });
});

var configurationBuilder = new ConfigurationBuilder()
                            .SetBasePath(builder.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add services to the container.

var defaultConnectionString = string.Empty;
defaultConnectionString = builder.Configuration.GetConnectionString("LocalConnection");

builder.Services.AddDbContext<MentoreContext>(
    options =>
    {
        options.UseSqlServer(defaultConnectionString);
        options.UseLazyLoadingProxies();
    }
);

var serviceProvider = builder.Services.BuildServiceProvider();

try
{
    var dbContext = serviceProvider.GetRequiredService<MentoreContext>();
    dbContext.Database.Migrate();
}
catch
{
}

///----
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.Run();