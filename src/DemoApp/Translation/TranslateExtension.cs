using System.Collections;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoApp.Translation
{
    /// <summary>
    /// The translateExtension ca be used in any XAML property to provide a value from a <see cref="ResourceManager"/>
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
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
                    // the key is not bound. Generate a key with the Framework name and and the property name
                    var targetDependencyProperty = providerValueTarget.TargetProperty as DependencyProperty;
                    var translationKey = this.key ?? $"{targetFrameworkElement.Name}_{targetDependencyProperty.Name}";
                }

                if (providerValueTarget.TargetObject is DependencyObject dependencyObject)
                {
                    // Since the target object is a dependency object it may be bound to the TranslationBindingSources Value property
                    // the BindingSopurce will send a
                    var binding = new Binding(nameof(TranslationBindingSource.Value))
                    {
                        Source = new TranslationBindingSource(() => this.FindTranslation(targetFrameworkElement, this.key))
                    };

                    if (targetFrameworkElement is not null && !targetFrameworkElement.IsLoaded)
                    {
                        // if the control isn't loaded yet it isn't connected to the logical tree.
                        // A translations source wont be found!
                        // Retry to find a translation source after the owning element is loaded.

                        RoutedEventHandler resetTranslationAfterLoad = null;

                        resetTranslationAfterLoad = delegate
                        {
                            // reset the cached value to make the source search for the translation again.
                            ((TranslationBindingSource)binding.Source).ResetCachedValue();

                            // this reconfiguration is only done once.
                            // remove the delegate form the framework element after it was processed.
                            targetFrameworkElement.Loaded -= resetTranslationAfterLoad;
                        };

                        targetFrameworkElement.Loaded += resetTranslationAfterLoad;
                    }

                    // return the biding as the value of this 'ProvideVvalue' call
                    return binding.ProvideValue(serviceProvider);
                }

                return default;
            });
        }

        private string FindTranslation(FrameworkElement targetFrameworkElement, string key)
        {
            FrameworkElement currentElement = targetFrameworkElement;
            ITranslationSource translationSource = null;

            // Ascend in the logical tree until a resource dictionary was found that contains an
            // element having a value of type ITranslationSource

            while (currentElement is not null && translationSource is null)
            {
                // search for ITranslationSource in the resources of the current element
                translationSource = currentElement
                    .Resources
                    .Cast<DictionaryEntry>()
                    .Where(de => de.Value is ITranslationSource)
                    .Select(de => de.Value as ITranslationSource)
                    .FirstOrDefault();

                // ascend in the logical tree
                currentElement = currentElement.Parent as FrameworkElement;
            }

            // Translate the key of give hint what is missing
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