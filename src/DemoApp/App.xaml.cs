using System.Windows;

namespace DemoApp;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;
    private MainWindow mainWindow;

    public App()
    {
        throw new InvalidOperationException(nameof(App));
    }

    public App(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        this.Resources.Add(nameof(IServiceProvider), this.serviceProvider);
        this.mainWindow = new MainWindow();
        this.mainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        this.mainWindow.Close();
    }
}