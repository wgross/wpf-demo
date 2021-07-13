using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
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

        static App()
        {
            ConfigureBootstrapLogger();
        }

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
            try
            {
                Log.Debug("Starting host");

                await this.host.StartAsync();
                this.Services.GetRequiredService<MainWindow>().Show();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Starting host failed");
            }
        }

        /// <summary>
        /// Stop and dispose the <see cref="host"/>
        /// </summary>
        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Log.Debug("Stopping host");

                using (this.host)
                    await this.host.StopAsync();
            }
            finally
            {
                Log.Information("Host stopped");

                // pending log messages a written to the sinks
                Log.CloseAndFlush();
            }
        }
    }
}