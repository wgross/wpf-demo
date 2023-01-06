using Microsoft.Toolkit.Mvvm.Messaging;
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
        /// Initializes a new instance of the <see cref="TranslationBindingSource"/> class.
        /// </summary>
        public TranslationBindingSource(Func<string> findTranslatedValue)
        {
            this.findTranslatedValue = findTranslatedValue;

            // Subscribe the culture change event. This event will be raised if the
            // Translation manager changes the current culture
            UICultureChangedEvent.Subscribe(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="TranslationBindingSource"/> is reclaimed by garbage collection.
        /// </summary>
        ~TranslationBindingSource() => UICultureChangedEvent.Unsubscribe(this);

        /// <summary>
        /// Returns either the currently cached value or fetches the translation
        /// from the translation source delegate.
        /// </summary>
        public object Value => this.cachedValue ??= this.findTranslatedValue();

        #region INotifyPropertyChanged

        /// <summary>
        /// Notifies the destination dependency object of a change of the language which will make it ask again
        /// for the new translation value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged

        #region INotifyCultureChanged

        /// <summary>
        /// Handles <see cref="UICultureChangedEvent"/>. The cahed value is reset the <see cref="PropertyChanged"/> event is raised.
        /// </summary>
        void IRecipient<UICultureChangedEventArgs>.Receive(UICultureChangedEventArgs message) => this.ResetCachedValue();

        internal void ResetCachedValue()
        {
            this.cachedValue = null;
            this.PropertyChanged?.Invoke(this, new(nameof(this.Value)));
        }

        #endregion INotifyCultureChanged
    }
}