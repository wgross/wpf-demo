using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoApp
{
    public abstract class PropertyBagViewModelBase : ObservableObject
    {
        private readonly Dictionary<string, object> propertyBag = new();

        /// <summary>
        /// Write <paramref name="value"/> to the property bag with the key in <paramref name="callerMemberName"/>.
        /// If value has changed or is written for the first time a <see cref="INotifyPropertyChanged"/> notification is raised.
        /// </summary>
        protected void SetPropertyValue<T>(T value, [CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
                throw new ArgumentNullException(nameof(callerMemberName));

            if (this.propertyBag.TryGetValue(callerMemberName, out var oldValue))
            {
                this.SetProperty(oldValue, value, model: this, callback: delegate
                {
                    // executed only if values are different
                    this.propertyBag[callerMemberName] = value;
                },
                propertyName: callerMemberName);
            }
            else
            {
                this.OnPropertyChanging(callerMemberName);
                this.propertyBag[callerMemberName] = value;
                this.OnPropertyChanged(callerMemberName);
            }
        }

        /// <summary>
        /// Read the value of <paramref name="callerMemberName"/> from the property bag and
        /// converts to <typeparamref name="T"/>.
        /// Returns default(T) if value wasn't set.
        /// </summary>
        protected T GetPropertyValue<T>([CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
                throw new ArgumentNullException(nameof(callerMemberName));

            if (this.propertyBag.TryGetValue(callerMemberName, out var value))
                return (T)value;
            else
                return default(T);
        }
    }
}