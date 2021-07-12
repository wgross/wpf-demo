using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        private IServiceProvider Services => this.host.Services;

        public App()
        {
            this.host = CreateGenericHost();
        }

        /// <summary>
        /// The Startup event handler replaces the <see cref="Application.StartupUri"/> to fetch the
        /// <see cref="MainWindow"/> from the <see cref="IServiceCollection"/>
        /// </summary>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await this.host.StartAsync();
            this.Services.GetRequiredService<MainWindow>().Show();
        }

        /// <summary>
        /// Stop and dispose the <see cref="host"/>
        /// </summary>
        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (this.host)
                await this.host.StopAsync();
        }
    }
}