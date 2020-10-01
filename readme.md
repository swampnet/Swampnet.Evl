master [![Build status](https://swampnet.visualstudio.com/Swampnet.Evl/_apis/build/status/Evl/CI%20-%20master%20(publish%20staging))](https://swampnet.visualstudio.com/Swampnet.Evl/_build/latest?definitionId=12)
develop [![Build status](https://swampnet.visualstudio.com/Swampnet.Evl/_apis/build/status/Evl/CI%20-%20develop)](https://swampnet.visualstudio.com/Swampnet.Evl/_build/latest?definitionId=16)

# Generic event logging service

### Back end
- Dot Net Core 2.0
- WebApi
- Entity Framework Core

Flexible, structured events
https://serilog.net/

http://www.mimekit.net/
A cross-platform .NET library for IMAP, POP3, and SMTP

@Notes
Create deployment script:
azure site deploymentscript -s Swampnet.Evl.sln --aspWAP Swampnet.Evl.Web\Swampnet.Evl.Web.csproj

## Configuration

Name                                         | Description
---------------------------------------------|--------------
````evl:schedule:trunc-events````            | timespan - When to truncate events (hh:mm)
````evl:trunc-events-timeout````             | timespan - 


