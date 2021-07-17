using System;
using System.Collections;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoApp.Translation
{
    /// <summary>
    /// The translateExtension ca be used in any XAML property to provide a value from a <see cref="ResourceManager"/>
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        private string key;

        public TranslateExtension(string key)
        {
            this.key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var getTargetElementSvc = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            return serviceProvider.InvokeAs<IProvideValueTarget, object>(providerValueTarget =>
            {
                var targetFrameworkElement = providerValueTarget.TargetObject as FrameworkElement;

                if (this.key is null)
                {
                    // the key isn ot bound. Generate a key with the Framework name and and the property name
                    var targetDependencyProperty = providerValueTarget.TargetProperty as DependencyProperty;
                    var translationKey = this.key ?? $"{targetFrameworkElement.Name}_{targetDependencyProperty.Name}";
                }

                if (providerValueTarget.TargetObject is DependencyObject dependencyObject)
                {
                    var binding = new Binding(nameof(TranslationBindingSource.Value))
                    {
                        Source = new TranslationBindingSource(() => this.FindTranslation(targetFrameworkElement, this.key))
                    };

                    if (targetFrameworkElement is not null && !targetFrameworkElement.IsLoaded)
                    {
                        // if the control isn'Ät loaded yet it isn't coinnected to teh logical tree.
                        // A translations source wont be foihnd yet.
                        // Retry to find a trabslation source after the owning element is loaded.

                        RoutedEventHandler resetTranslationAfterLoad = null;

                        resetTranslationAfterLoad = delegate
                        {
                            ((TranslationBindingSource)binding.Source).ResetCachedValue();
                            targetFrameworkElement.Loaded -= resetTranslationAfterLoad;
                        };

                        targetFrameworkElement.Loaded += resetTranslationAfterLoad;
                    }

                    return binding.ProvideValue(serviceProvider);
                }

                return default;
            });
        }

        private string FindTranslation(FrameworkElement targetFrameworkElement, string key)
        {
            FrameworkElement currentElement = targetFrameworkElement;
            ITranslationSource translationSource = null;

            // Ascend in the logical tree untils a resource dictionary was found that contains an
            // elemenet having a value of type ITranslationSource

            while (currentElement is not null && translationSource is null)
            {
                // search for ITRanslationSource in currentElement
                translationSource = currentElement
                    .Resources
                    .Cast<DictionaryEntry>()
                    .Where(de => de.Value is ITranslationSource)
                    .Select(de => de.Value as ITranslationSource)
                    .FirstOrDefault();

                // ascend in the logical tree
                currentElement = currentElement.Parent as FrameworkElement;
            }

            // Trabslat the key of give hint whet is missing
            if (translationSource is null)
                return "[missing:translation source]";

            return translationSource.GetString(key) ?? $"[missing:{key}]";
        }
    }

    internal static class ServiceProviderExtsnsions
    {
        public static R InvokeAs<T, R>(this IServiceProvider serviceProvider, Func<T, R> invoke)
        {
            if (serviceProvider.GetService(typeof(T)) is T service)
            {
                return invoke(service);
            }
            else return default;
        }
    }
}