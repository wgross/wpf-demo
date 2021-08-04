using System;
using System.Windows;

namespace DemoApp
{
    public partial class App : Application
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private MainWindow mainWindow;

        public App()
        {
            throw new InvalidOperationException(nameof(App));
        }

        public App(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.mainWindow = new MainWindow(this.mainWindowViewModel);
            this.mainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.mainWindow.Close();
        }
    }
}