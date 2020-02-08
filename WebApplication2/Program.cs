using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace WebApplication2
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Create the Serilog logger, and configure the sinks
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    // Filter out ASP.NET Core infrastructre logs that are Information and below
            //    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            //    .Enrich.FromLogContext()
            //    .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Events)
            //    //.WriteTo.Console()
            //    //.WriteTo.Seq("http://localhost:5341")
            //    .CreateLogger();

            // Wrap creating and running the host in a try-catch block
            //try
            //{
            //    //Log.Information("Starting host");
            CreateHostBuilder(args).Build().Run();
            return 0;
            //}
            //catch (Exception ex)
            //{
            //    //Log.Fatal(ex, "Host terminated unexpectedly");
            //    return 1;
            //}
            //finally
            //{
            //    //Log.CloseAndFlush();
            //}
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureLogging((hostingContext, logging) => logging.ClearProviders())
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                            telemetryConfiguration.InstrumentationKey = hostingContext.Configuration["ApplicationInsights:InstrumentationKey"];

                            loggerConfiguration
                                .MinimumLevel.Information()
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                .MinimumLevel.Override("System", LogEventLevel.Warning)
#if DEBUG
                        .MinimumLevel.Verbose()
                                .WriteTo.Console()
                                .WriteTo.Debug()
                                .WriteTo.Seq("http://localhost:5341")       // docker run --rm -it -e ACCEPT_EULA=Y -p 5341:80 datalust/seq
#endif
                        .Enrich.FromLogContext()
                                .Enrich.WithMachineName()
                                .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
                        })
                        .UseStartup<Startup>();
                });
    }
}
