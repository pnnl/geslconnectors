# GESL_connectors

## For C# code add `config.cs` file with credentials:

`namespace siglib_demo`

`{`

    `internal class siglib_config`

    `{`
        `public static string email = "your email";`

        `public static string apikey = "your API key";`

        `public static string proxy = "your proxy";`

`    }`

`}`


## For MATLAB code add `config.m` file with credentials:

`function cfg = config()`

    `cfg.email = 'your email';`
    `cfg.apikey = 'your API key';`
    
`end`