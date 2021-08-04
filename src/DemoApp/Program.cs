using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace DemoApp
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            ConfigureBootstrapLogger();

            using var host = CreateGenericHost(args);
            try
            {
                host.Start();

                var app = host.Services.GetRequiredService<App>();
                app.InitializeComponent();
                app.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal("Startup Failed:", ex);

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHost CreateGenericHost(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseSerilog(ConfigureRuntimeLogger)
                .ConfigureServices(ConfigureAppServices)
                .Build();
        }

        private static void ConfigureAppServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<Translation.TranslationExampleViewModel>();
            services.AddSingleton<Validation.ValidationExampleViewModel>();
        }

        /// <summary>
        /// Enable logging until the config file was read and the app is runnning.
        /// </summary>
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