﻿# Features of the App

## Dependency Injection in App

The `App` main class of the WPF-demo initializes a *Generic Host* instance providing logging, configuration and dependency injection.
The `App` class publishes the `IServiceProvider` as a resource to the other controls of the application from where it can be retrieved by any `FrameworkElement` using its
`FindResource` method.

### Variant 1: Creating a Generic Host inside of App

(see at [Github](https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs))

To make this working th XAML of the App was modified:
- StartupUri was removed
- Startup event handler was introduced showing the main window instance provided by the dependency injection container
- Exit event handler was introduced disposing and stopping the generic host

### Variant 2: Using an alternative Entry Point

(see at https://stackoverflow.com/questions/6156550/replacing-the-wpf-entry-point)

- A Program class having a static Main method is introduced. 
It creates the generic host in the same way as a console app would.
- The project file explicitly points to the new entry point with `<StartupObject>DemoApp.Program</StartupObject>`.
- Opening and closing the main window is done manually in the App class using the startup and exit events.
- It is required that the main method is marked as `[STAThread]`.
Also it should not be `async` because the would create another thread not being created as as single thread apartment.
- The dependency injection container provides an instance of the `App` class having the view model of the main window injected.

While this is more code and slightly more complicated it is the more flexible. 

## Running in a Console if required

This extends the startup of the app as described above. 
The STAThread attribute is removed from the App main class. 
Instead the thread hosting the Application is started manually with the apartment set to STA: ```thread.SetApartmentState(ApartmentState.STA)```.

Access to a console is either achieved by allocation a new one or attaching to the parent console in method ```RunAsConsole```
The Win32 API calls to show a console or to detach the console from the process are implemented in [Hosting/Win32](./Hosting/Win32.cs).

## Serilog Provides Structured Logging

While the application code uses Microsofts logging abstraction as API to send log messages, Serilog is the underlying logging provider. 
Serilog is configured to write the log events in a structured and machine readable format (JSON).
This allows to filter log messages by referring to properties or other meta data annotating the original log message. 

- [Bootstrap Logging/Serilog.Extensions.Hosting](https://github.com/serilog/serilog-extensions-hosting)
- [File Sink and JSON Formatter](https://github.com/serilog/serilog-sinks-file)

## Dynamic Translation

The demonstrator shows an approach to change the UI language dynamically. 
Translations from the Resources follow the culture change. 
Details in [Translation/readme.me](Translation/readme.md).

## PropertyBagViewModelBase for simple Property Notifications

In many cases a simple generic implementation of a notifying property value store is sufficient.
The base class [PropertyBagViewModelBase](PropertyBagViewModelBase.cs) holds an instance of 
```Dictionary<string,object>``` to store property values using the methods ```SetValue(T)``` and ```GetValue<T>()```.
The name of the property is provided by the C# compiler using a parameter attributed by ```[CallerMemberName]```.

To raise notification the methods of the base class ```ObservableObject``` from WPF community extensions are used.

## Host a Web Service in WPF

The demonstrator shows an approach to provide a web service API (`http://localhost:5000/data`). 
This is a fringe case but may be useful to make a software accessible from PowerShell (`Invoke-RestMethod`) for automation.

This requires a reference to the web framework in the project file: 
```xml
<ItemGroup>
	<FrameworkReference Include="Microsoft.AspNetCore.App" />
</ItemGroup>
```

The web uses minimal API and is hosted as a `HostedService` implemented in [./WebService](./WebService/HostedWebApp.cs).
Even static web pages could be served by setting the `WebRootPath` to the pages directory and the static file middleware to the web host.

(see also [github issue](https://github.com/dotnet/runtime/issues/31012))


  

