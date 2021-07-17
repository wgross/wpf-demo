using System;
using System.ComponentModel;

namespace DemoApp.Translation
{
    /// <summary>
    /// Hold the translated data depending of the currently set language.
    /// It provides
    /// </summary>
    /// <remarks>
    /// Based on Christian Mosers localization proposal for WPF applications: http://www.wpftutorial.net/LocalizeMarkupExtension.html
    /// </remarks>
    public sealed class TranslationBindingSource : INotifyPropertyChanged, INotifyCultureChanged
    {
        private readonly Func<object> findTranslatedValue;
        private object cachedValue = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationData"/> class.
        /// </summary><
        public TranslationBindingSource(Func<string> findTranslatedValue)
        {
            this.findTranslatedValue = findTranslatedValue;

            UICultureChangedEvent.Subscribe(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="TranslationData"/> is reclaimed by garbage collection.
        /// </summary>
        ~TranslationBindingSource() => UICultureChangedEvent.Unsubscribe(this);

        public object Value => this.cachedValue ??= this.findTranslatedValue();

        internal void ResetCachedValue()
        {
            this.cachedValue = null;
            this.PropertyChanged?.Invoke(this, new(nameof(this.Value)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region INotifyCultureChanged

        public void Receive(UICultureChangedEventArgs message) => this.ResetCachedValue();

        #endregion INotifyCultureChanged
    }
}