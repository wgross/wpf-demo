# Features of the App

## Dependency Injection in App

The App main class of the WPF-demo initializes a *Generic Host* instance providing logging, configuration and dependency injection.
(see at [Github](https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs))

To make this working th XAML of the App was modified:
- StartupUri was removed
- Startup event handler was introduced showing the main window instance provided by the dependency injection container
- Exit event handler was introduced disposing and stopping the generic host

