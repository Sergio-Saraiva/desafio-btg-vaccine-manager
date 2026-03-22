using VaccineManager.Api.Middlewares;
using VaccineManager.Application.Common.Settings;
using VaccineManager.IOC;

namespace VaccineManager.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<AppSettings>(
            builder.Configuration
        );
        // Add services to the container.
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.RegisterServices(builder.Configuration);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vaccine Manager API");
            });
            app.MapGet("/", async context =>
            {
                context.Response.Redirect("/swagger");
                await Task.CompletedTask;
            });
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization(); 
        app.MapControllers();

        app.Run();
    }
}