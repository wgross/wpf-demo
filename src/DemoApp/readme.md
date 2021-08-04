# Features of the App

## Dependency Injection in App

The App main class of the WPF-demo initializes a *Generic Host* instance providing logging, configuration and dependency injection.

### Variant 1: CReatinig a Generic Host inside of App

(see at [Github](https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs))

To make this working th XAML of the App was modified:
- StartupUri was removed
- Startup event handler was introduced showing the main window instance provided by the dependency injection container
- Exit event handler was introduced disposing and stopping the generic host

### Variant 2: Using a alternative Entry Point

(see at https://stackoverflow.com/questions/6156550/replacing-the-wpf-entry-point)

- A Program class having a static Main method is introduced. 
It creates the generic host in the same way as a console app would.
- The project file explicitely points to the new entry point with `<StartupObject>DemoApp.Program</StartupObject>`.
- Opening and closing the main window is deon manually in the App class using the startup and exit events.
- It is required that the main method is marked as `[STAThread]`.
Also it should not be `async` because the would create another thread not being created as as single thread appartment.
- The dependency injection container provides an instance of the `App` class having the view model of the main window injected.

While this is more code and slightly more complicated it is the more flexible. 
It would be possible to add consaole app features by adding a command line args interpreter.
The project already contains the Win32 api calls to show a console or to detach teh console from the process in [Hosting/Win32](./Hosting/Win32.cs)

## Serilog Provides Structured Logging

While the application code uses Microsofts logging abstraction as API to send log messages, Serilog is the underlying logging provider. 
Serilog is configured to write the log events in a structured and machine readable format (JSON).
This allows to filter log messages by referring to properties or other metadata annotating the original log message. 

- [Bootstrap Logging/Serilog.Extensions.Hosting](https://github.com/serilog/serilog-extensions-hosting)
- [File Sink and Json Formatter](https://github.com/serilog/serilog-sinks-file)

## Dynamic Translation

The demonstrator shows an approach to change the ui language dynamically. 
Translations from the Resources foillow the culture change. 
Details in [Translation/readme.me](Translation/readme.md).