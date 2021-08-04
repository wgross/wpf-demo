using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DemoApp
{
    public partial class App
    {
        private static IHost CreateGenericHost(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseSerilog(ConfigureRuntimeLogger)
                .ConfigureServices(AppServices)
                .Build();
        }

        private static void AppServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<Translation.TranslationExampleViewModel>();
            services.AddSingleton<Validation.ValidationExampleViewModel>();
        }
    }
}