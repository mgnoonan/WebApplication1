using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OwaspHeaders.Core.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) =>
    {
        var client = services.GetRequiredService<TelemetryClient>();

        loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
#if DEBUG
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq("http://localhost:5341")       // docker run --rm -it -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
#endif
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.ApplicationInsights(client, TelemetryConverter.Traces);
    });

    builder.Services.AddSingleton<AppVersionInfo>();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddApplicationInsightsTelemetry();
    var app = builder.Build();

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");

        // Add recommended OWASP security headers
        app.UseSecureHeadersMiddleware(SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration());
    }

    app.MapControllers();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest);

    app.UseRouting();

    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete");
    Log.CloseAndFlush();
}
