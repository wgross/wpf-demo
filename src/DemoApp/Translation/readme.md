﻿# Translations

Wherever a translated value is needed a markup extension implemented in [_TranslateExtension_](TranslateExtension.cs) may be used in XAML code.
To fetch the value of the resource identified with 'MyTranslationKey' you put:
```XAML
<TextBlock Text="{tr:Translate MyTranslationKey}"/>
```
The key may be omitted:
```XAML
<TextBlock Name="textBox" Text="{tr:Translate}"/>
```
In this case the resource-key would be created from the name of the control and the property name: 'textBox_Text' would be looked up.

The markup extension generates a WPF data binding which is given as the value to the translated dependency property. 
The binding will send a *INotifyPropertyChanged* notification if the current UI cultures changes.

## Configurable Translation Source

The source of the translation value is determined by the first instance of the interface *ITranslationSource* that is found by the traslation extensions.
The search will start at the resource dictionary of the control where the extension is placed and will continue upwards in the logical tree.
I this example the resource dictionary of the _MainWindow_ provides the translation source.

```XAML
<Window.Resources>
   <tr:ResourceDictionaryTranslationSource x:Key="translationSource" ResourceManager="{x:Static tr:Translations.ResourceManager}" />
</Window.Resources>
```
As an implemenation sample for _ITranslationSource_ the [ResourceManagerTranslationSource](ResourceManagerTranslationSource.cs) is provided. 
It is initialized in the [MainWindow](../MainWindow.xaml) with the static _ResourceManager_ generated by the [Translations.resx](Translations.resx) code generator.

## React on UI Culture change

The notification of a culture change depends on the messaging implemented by the MVVMToolKits 
[_WeakReferenceMessenger_](https://github.com/CommunityToolkit/WindowsCommunityToolkit/blob/ee5a7b863ef8600e837f30a7a9ef4590c3f14b31/Microsoft.Toolkit.Mvvm/Messaging/WeakReferenceMessenger.cs).

It may be accessed by a static facade: [UICultureChangedEvent](UICultureChangedEvent.cs). 
Setting the UI Culture using this facade raises the event forcing the _TranslationBindingSource_ to fetch its value again.
The following snippet is taken from the [TranslationExampleViewModel](TranslationExampleViewModel.cs).

```CSharp
if (this.SetProperty(ref this.selectedCulture, value, nameof(SelectedCulture)))
{
    // send the global event to refresh all translations
    UICultureChangedEvent.SetCurrentUICulture(value.Culture);
}
```

## References

* [Mosers WPF-Tutorial about Localization](http://www.wpftutorial.net/LocalizeMarkupExtension.html) discusses the general idea.
* [XAMLMarkupExtensions on github](https://github.com/XAMLMarkupExtensions) with a similar approach and generally interesting things about markup extensions.
* [Microsofts Docs about Markup-Extensions](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/advanced/markup-extensions-and-wpf-xaml).
* [Use of RESX files for localization](https://www.tutorialspoint.com/wpf/wpf_localization.htm) explained.
* [MvvmToolkit on github](https://github.com/CommunityToolkit/WindowsCommunityToolkit) providing weak reference messaging.