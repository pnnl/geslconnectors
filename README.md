# GESL_connectors

Welcome to GESL_connectors! This tool allows you to seamlessly integrate GESL's APIs into your C# or MATLAB projects. Before you can begin, you'll need to obtain an API key by registering with the GESL platform.

## Getting Started

### Registration
First, register at [GESL](https://gsl.ornl.gov/) to obtain your API key. This key will enable you to access our suite of APIs.

### Configuration

#### C# Projects
For C# applications, create a `config.cs` file in your project with the following credentials:

```csharp
namespace siglib_demo
{
    internal class siglib_config
    {
        public static string email = "your email";
        public static string apikey = "your API key";
        public static string proxy = "your proxy";
    }
}
```

#### MATLAB
For MATLAB code add `config.m` file in your project with the following credentials:

```MATLAB
function cfg = config()
    cfg.email = 'your email';
    cfg.apikey = 'your API key';   
end
```