# GESL_connectors

Welcome to GESL_connectors! This tool allows you to seamlessly integrate GESL's APIs into your C# or MATLAB projects. Before you can begin, you'll need to obtain an API key by registering with the GESL platform.

## Getting Started

### Registration
First, register at [GESL](https://gsl.ornl.gov/) to obtain your API key. This key will enable you to access GESL APIs.



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

## Description of the connectors
### C#
The Visual Studio solution consists of two projects:
- `sig_lib` - A library (DLL) that includes functions to interact with the GESL API.
- `siglib_demo` - A demo project showing how to use the library functions.

### MATLAB

The MATLAB folder includes a set of functions:

- `get_event_tags.m` - Retrieves tags associated with events.
- `get_event_ids.m` - Obtains IDs for specific events.
- `get_event_data.m` - Fetches data for given event IDs.
- `request_data.m` - Sends a request for data based on specified parameters.

To test the functionality of the MATLAB connectors, run `test_api.m`. This script identifies events for specific tags, downloads them as ZIP files, extracts CSV files from the archives, and generates a plot.
