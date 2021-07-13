# Features of the App

## Dependency Injection in App

The App main class of the WPF-demo initializes a *Generic Host* instance providing logging, configuration and dependency injection.
(see at [Github](https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs))

To make this working th XAML of the App was modified:
- StartupUri was removed
- Startup event handler was introduced showing the main window instance provided by the dependency injection container
- Exit event handler was introduced disposing and stopping the generic host

## Serilog Provides Structured Logging

While the application code uses Microsofts logging abstraction as API to send log messages, Serilog is the underlying logging provider. 
Serilog is configured to write the log events in a structured and machine readable format (JSON).
This allows to filter log messages by referring to properties or other metadata annotating the original log message. 

- [Bootstrap Logging/Serilog.Extensions.Hosting](https://github.com/serilog/serilog-extensions-hosting)
- [File Sink and Json Formatter](https://github.com/serilog/serilog-sinks-file)
