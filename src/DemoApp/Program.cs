using DemoApp.Hosting;
using DemoApp.WebService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DemoApp;

public static class Program
{
    public static async Task<int> Main(bool console = false, string[] args = null)
    {
        ConfigureBootstrapLogger();

        try
        {
            if (console)
            {
                await RunAsConsole(args);
            }
            else
            {
                await RunAsWpfApplication(args);
            }

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

    private static async Task RunAsConsole(string[] args)
    {
        Win32.SetStdHandle(Win32.StdOutputHandle, IntPtr.Zero);

        if (!Win32.AttachToParentConsole())
            Win32.AllocConsole();

        using var host = CreateGenericHost(args);

        //host.Start();

        Console.WriteLine($"Hello World! {(args is not null ? string.Join(" ", args) : string.Empty)}");

        await host.StopAsync();
    }

    private static async Task RunAsWpfApplication(string[] args)
    {
        Win32.FreeConsole();

        using var host = CreateGenericHost(args);

        await host.StartAsync();

        TaskCompletionSource wpfAppCompleted = new();

        var thread = new Thread(new ThreadStart(() =>
        {
            var app = host.Services.GetRequiredService<App>();
            app.InitializeComponent();
            app.Run();

            wpfAppCompleted.SetResult();
        }));

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        await wpfAppCompleted.Task;
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
        services.AddSingleton<ListTree.ListTreeExampleViewModel>();
        services.AddSingleton<ItemsCanvas.ItemsCanvasViewModel>();
        services.AddHostedService<HostedWebApp>();
    }

    /// <summary>
    /// Enable logging until the config file was read and the app is running.
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