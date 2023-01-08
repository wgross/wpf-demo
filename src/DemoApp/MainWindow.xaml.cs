using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DemoApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Loaded += this.MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var serviceProvider = this.FindResource(nameof(IServiceProvider)) as IServiceProvider;

        this.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
    }
}