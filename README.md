# Description

A couple of extension methods for adding log4net support to ASP.NET 5. This currently only supports dnx451 (full .NET framework) as log4net does not have CoreCLR support yet.

# Usage


## 1. Add a reference

Inside of project.json, add a reference to dotnetliberty.AspNet.log4net:

```javascript
{
  "webroot": "wwwroot",
  "userSecretsId": "aspnet5-AspNet5Log4Net-96932d2b-063c-4568-a826-82f718a33f40",
  "version": "1.0.0-*",

  "dependencies": {
    // ... 
    "dotnetliberty.AspNet.log4net": "1.0.0-beta8"
  },
```

## 2. log4net XML Configuration

With ASP.NET 5 we no longer have a web.config file. Instead we should create an XMLile at the root of our project that contains just our log4net configuration. For example:

**Notice** that we are using a pattern for the file name. The value of the property `appRoot` is provided at runtime by this library.


```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="RollingFile" type="log4net.Appender.FileAppender">
    <file type="log4net.Util.PatternString" value="%property{appRoot}\app.log" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%-5p %d{hh:mm:ss} %message%newline" />
    </layout>
  </appender>

  <root>
    <level value="DEBUG" />
    <appender-ref ref="RollingFile" />
  </root>
</log4net>
```


## 3. Configure log4net at Startup

Add an extra line in the Startup.cs constructor to tell it where to find the log4net XML file:

```csharp
public class Startup
{
    public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
    {
        // Setup configuration sources.

        var builder = new ConfigurationBuilder()
            .SetBasePath(appEnv.ApplicationBasePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        appEnv.ConfigureLog4Net("log4net.xml");

        // ...
    }
    // ...
```

## 4. Register provider with ILoggerFactory

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.MinimumLevel = LogLevel.Verbose;
    loggerFactory.AddConsole();
    loggerFactory.AddDebug();

    loggerFactory.AddLog4Net();
    // ...
```

## 5. Done

Now you will be able to use the Microsoft Logging framework throughout your application, and log4net will be used as a logging provider (based on the configuration provided in log4net.xml).

```csharp
var logger = loggerFactory.CreateLogger("Test");
logger.LogInformation("This will get written to app.log via log4net.");
```