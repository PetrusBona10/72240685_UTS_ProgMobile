using AcademicApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var bodyParameter = context.ActionDescriptor.Parameters
                .FirstOrDefault(parameter => parameter.BindingInfo?.BindingSource == BindingSource.Body);

            var hasBindingError = context.ModelState.Any(entry =>
                entry.Key.StartsWith("$", StringComparison.Ordinal)
                || (bodyParameter is not null && entry.Key == bodyParameter.Name)
                || entry.Value?.Errors.Any(error => error.Exception is not null) == true);

            if (hasBindingError)
            {
                return new BadRequestObjectResult(new { message = "Data yang dikirim tidak valid." });
            }

            var messages = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .Select(entry => entry.Value!.Errors
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                        ? "Data yang dikirim tidak valid."
                        : error.ErrorMessage)
                    .First())
                .Distinct()
                .ToList();

            var message = messages.Count > 0
                ? string.Join(" ", messages)
                : "Data yang dikirim tidak valid.";

            return new BadRequestObjectResult(new { message });
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AcademicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AcademicDb")));

builder.Services.AddScoped<IDosenDAL, DosenDAL>();
builder.Services.AddScoped<IMahasiswaDAL, MahasiswaDAL>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Total-Count");
    });
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            message = "Terjadi kesalahan pada server. Silakan coba lagi."
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "Academic API v1");
        options.DocumentTitle = "Academic API";
    });
    app.UseCors("DevelopmentCors");
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AcademicDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

app.Run();
