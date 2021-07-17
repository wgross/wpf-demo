using Microsoft.Toolkit.Mvvm.Messaging;
using System.Globalization;
using System.Threading;

namespace DemoApp.Translation
{
    public record UICultureChangedEventArgs(CultureInfo Old, CultureInfo New);

    /// <summary>
    /// The interface is implemented by classes which require notfication of a change of the UI culture
    /// </summary>
    public interface INotifyCultureChanged : IRecipient<UICultureChangedEventArgs>
    {
    }

    /// <summary>
    /// The static facade for subscribing and sending the <see cref="UICultureChangedEventArgs"/> event.
    /// </summary>
    internal static class UICultureChangedEvent
    {
        public static void SetCurrentUICulture(CultureInfo uiCulture)
        {
            if (uiCulture.Name != Thread.CurrentThread.CurrentUICulture.Name)
            {
                var old = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentUICulture = uiCulture;

                WeakReferenceMessenger.Default.Send(new UICultureChangedEventArgs(old, uiCulture));
            }
        }

        internal static void Unsubscribe(INotifyCultureChanged subscriber) => WeakReferenceMessenger.Default.Unregister<UICultureChangedEventArgs>(subscriber);

        internal static void Subscribe(INotifyCultureChanged subscriber) => WeakReferenceMessenger.Default.Register(subscriber);
    }
}