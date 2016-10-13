# Owin.Logging.LibLog
LibLog factory implementation for OWIN Logging

## Installation
Similar to nuget install of LibLog. File is added to `App_Start\LibLog.4.2\Owin.Logging.LibLog.cs` under namespace `YourRootNamespace.Logging` (YourRootNamespace being replaced with your root namespace of project). It is expected that LibLog is installed under the same namespace, else you will have to rename the namespace.

## Using
Configure LibLog and underlying Log framework as you usually might.

Then to utilize LibLog in Owin, use extension methods:

```C#
using Owin.Logging.Common;

    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseLibLogging();
        }
    }
```

Overloaded methods are available to control the transform of TraceEventType to levels in LibLog.LogLevel

Default transform is:

| TraceEventType	| LibLog Loglevel |
|-----------------|---------------|
| Critical        | Fatal			  	|
| Error			    	| Error 		  	|
| Warning			  	| Warn 		  		|
| Information		  | Info 			  	|
| Verbose			  	| Trace 	  		|
| Start				  	| Debug 		  	|
| Stop				  	| Debug 		  	|
| Suspend			  	| Debug 		  	|
| Resume			  	| Debug 		  	|
| Transfer			  | Debug 		  	|
