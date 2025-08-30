using System.Text.Json.Serialization;
using FimMind.Application.Accounts.Commands;
using FimMind.Application.Interfaces;
using FinMind.API.Filters;
using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Infrastructure.Data.Context;
using FinMind.Infrastructure.Data.Seeders;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => { options.Filters.Add<ApiExceptionFilterAttribute>(); })
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FinMind", Version = "v1" });
});

builder.Services.AddDbContext<FinMindDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<FinMindDbContext>());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(typeof(CreateAccountCommandHandler).Assembly);

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateAccountCommand).Assembly);


//OpenTelemetry настройка
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FinMind.Api"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://jaeger:4317");
            });
    })
    .WithMetrics(metrics => metrics
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FinMind.Api"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddPrometheusExporter());


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinMindDbContext>();
    context.Database.Migrate();
    DefaultUserSeeder.SeedData(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Configure Swagger with Docker support
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "swagger/{documentName}/swagger.json";
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer 
                { 
                    Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Description = "Docker container"
                }
            };
        });
    });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = "swagger";
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app.Run();

