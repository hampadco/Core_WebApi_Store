using Serilog;
using shop.Framework.Infrastructure.Extension;
using Serilog.Formatting.Compact;
using shop.Service.Extension.FileUtil.Interfaces;
using shop.Service.Extension.FileUtil.Services;
using Shop.Api.Infrastructure.JwtUtil;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using shop.Data.ApplicationContext;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationServices(builder.Configuration);

//CORS allow all
builder.Services.AddCors(options =>
{
    options.AddPolicy("Shop", builder =>
    {
        builder.WithOrigins("http://localhost:5179/").AllowAnyMethod().AllowAnyHeader();
    });
});









builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "1568.Shop.Api", Version = "v1" });
});


builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.RegisterApiDependency(builder.Configuration);
// builder.Host.UseSerilog(((context, provider, logger) =>
// {
//     logger.MinimumLevel.Information().WriteTo.File("log.txt",
//         rollingInterval: RollingInterval.Day,
//         rollOnFileSizeLimit: true, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
//     ).WriteTo.File(new RenderedCompactJsonFormatter(), "log.ndjson", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning);
// }));



var app = builder.Build();


//CORS
app.UseCors("Shop");
app.UseIpRateLimiting();
app.UseStaticFiles();
app.UseDefaultFiles();
app.ConfigureRequestPipeline();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});



app.Run();
