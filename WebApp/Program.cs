﻿using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using EnochTelemetry;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RazorClassLib.Data;
using RazorClassLib.Services;
using WebApp.Components;
using WebApp.EnochTelemetry;
using WebApp.Services;

public partial class Program()
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();


        builder.Services.AddDbContextFactory<TicketContext>(config => config.UseNpgsql(builder.Configuration["pec_tickets"]));
        builder.Services.AddSingleton<IOccasionService, OccasionService>();
        builder.Services.AddSingleton<ITicketService, TicketService>();
        builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();
        builder.Services.AddHealthChecks();

        builder.Services.AddLogging();

        const string serviceName = "enochservice";

        builder.Logging.AddOpenTelemetry(options =>
        {
            options
                .SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddService(serviceName))
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(builder.Configuration["COLLECTOR_URL"] ?? throw new NullReferenceException("environment variable not set: COLLECTOR_URL"));
                })
            .AddConsoleExporter();
        });

        builder.Services.AddOpenTelemetry()
              .ConfigureResource(resource => resource.AddService(serviceName))
              .WithTracing(tracing => tracing
                  .AddSource(EnochTraces.OccasionsSource)
                  .AddSource(EnochTraces.TicketsSource)
                  .AddAspNetCoreInstrumentation()
                  .AddConsoleExporter()
                  .AddOtlpExporter(o =>
                    o.Endpoint = new Uri("http://otel-collector:4317/")))
              .WithMetrics(metrics => metrics
                  .AddAspNetCoreInstrumentation()
                  .AddMeter(EnochMetrics.MetricsName)
                  .AddConsoleExporter()
                  .AddOtlpExporter(o =>
                    o.Endpoint = new Uri("http://otel-collector:4317/")));

        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = factory.CreateLogger("Program");
        LogStartupMessage(logger, "fun");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //comment
            app.UseHsts();
        }

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
        });

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
    [LoggerMessage(Level = LogLevel.Information, Message = "Hello World! Logging is {Description}.")]
    public static partial void LogStartupMessage(ILogger logger, string description);
}



