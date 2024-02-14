// based on : https://gist.github.com/huoxudong125/56a2e1fe5b1916a01064668a3397a57f

using System.Windows;

namespace DemoApp.ItemsCanvas;

public static class ActualSizeObserver
{
    public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
        "Observe",
        typeof(bool),
        typeof(ActualSizeObserver),
        new FrameworkPropertyMetadata(OnObserveChanged));

    public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
        "ObservedWidth",
        typeof(double),
        typeof(ActualSizeObserver));

    public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
        "ObservedHeight",
        typeof(double),
        typeof(ActualSizeObserver));

    public static bool GetObserve(FrameworkElement frameworkElement) => (bool)frameworkElement.GetValue(ObserveProperty);

    public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        => frameworkElement.SetValue(ObserveProperty, observe);

    public static double GetObservedWidth(FrameworkElement frameworkElement)
        => (double)frameworkElement.GetValue(ObservedWidthProperty);

    public static void SetObservedWidth(FrameworkElement frameworkElement, double observedWidth)
        => frameworkElement.SetValue(ObservedWidthProperty, observedWidth);

    public static double GetObservedHeight(FrameworkElement frameworkElement)
        => (double)frameworkElement.GetValue(ObservedHeightProperty);

    public static void SetObservedHeight(FrameworkElement frameworkElement, double observedHeight)
        => frameworkElement.SetValue(ObservedHeightProperty, observedHeight);

    private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var frameworkElement = (FrameworkElement)dependencyObject;

        if (e is { NewValue: true })
        {
            frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
            UpdateObservedSizesForFrameworkElement(frameworkElement);
        }
        else
        {
            frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
        }
    }

    private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
    }

    private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
    {
        frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
        frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);
    }
}