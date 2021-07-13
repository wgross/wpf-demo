using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DemoApp
{
    public partial class App
    {
        private static IHost CreateGenericHost()
        {
            return Host
                .CreateDefaultBuilder()
                .UseSerilog(ConfigureRuntimeLogger)
                .ConfigureServices(AppServices)
                .Build();
        }

        private static void AppServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}