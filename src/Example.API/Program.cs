using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

ConfigureLogging();

try
{
    Log.Information("Starting host");

    BuildAndRun();

    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");

    return -1;
}
finally
{
    Log.CloseAndFlush();
}

void BuildAndRun()
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    builder.WebHost.UseUrls($"http://*:{port}");

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHealthChecks();

    var app = builder.Build();

    if (app.Environment.IsDevelopment()) { }

    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseRouting();

    app.MapControllers();
    app.MapHealthChecks("/healthcheck");

    app.Run();
}

static void ConfigureLogging()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Filter.ByExcluding("RequestPath like '%/healthcheck%'")
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Example.API")
        .WriteTo.Console(new JsonFormatter())
        .CreateLogger();
}
