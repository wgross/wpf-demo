using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace DemoApp
{
    public partial class App
    {
        private static void ConfigureBootstrapLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log-bootstrap.log")
                .CreateBootstrapLogger();
        }

        private static void ConfigureRuntimeLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
        }
    }
}