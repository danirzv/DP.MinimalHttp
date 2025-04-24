

# DP.MinimalHttp
🚀 *The simplest way to use `HttpClient` in .NET — with automatic JSON deserialization and zero boilerplate.*

[![NuGet](https://img.shields.io/nuget/v/DP.MinimalHttp.svg)](https://www.nuget.org/packages/DP.MinimalHttp)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Features
- ✅ **Zero-config** `HttpClient` setup.
- ✅ **Automatic JSON deserialization** into `SuccessModel` or `ErrorModel`.
- ✅ **Minimal API**: Just 2-3 methods to handle all HTTP requests.

## Getting Started

1.Install `DP.MinimalHttp` using cli or NuGet plugin of your IDE

```bash
  dotnet add package DP.MinimalHttp
```

2.Register your HttpClientOptions and HttpClient

#### appsettings.json
```json
{
  "TargetHttpClientOptions": {
    "BaseUri": "www.Foo.com",
    "HttpClientLoggingMode": 3, // *Optional* 3 means Request and Response will be logged
    "HealthCheckPath": "/healthz", // *Optional*
    "Timeout": "00:01:00", // *Optional*
    "CertificateThumbprint": "b8bb81876833873942045a8df8f06219e00602ebcb4384c7abc24f18379c87f5", // *Optional* IgnoreCertificate will surpress this option 
    "IgnoreCertificate": true // *Optional*
  }
}
```

#### TargetHttpClientOptions.cs
```csharp
    public class TargetHttpClientOptions : HttpClientOptions
    {}
```

#### Program.cs
```csharp
    // Setup TargetHttpClientOptions from your appsettings.json 
    services.Configure<TargetHttpClientOptions>(
        configuration.GetSection("TargetHttpClientOptions")
    );

    // Register an HttpClient for class 'TargetClient'
    services.AddHttpClient<TargetClient, TargetHttpClientOptions>();
```

3.You are good to go! use your httpClient!

#### TargetClient.cs
```csharp
    public class TargetClient
    {
        private readonly HttpClient _client;
        private readonly Ilogger _logger;
        public TargetClient(ILogger<TargetClient> logger, HttpClient client)
        {
            _client = client;
            _logger = logger;
        }

        public Task<TargetSuccessModel> CoolMethodCall()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/Bar");
                TargetSuccessModel response = await _client.SendAsync<TargetSuccessModel, TargetErrorModel>(request, _logger);

                return response;
            }
            catch (ExternalProviderException<TargetErrorModel> ex)
            {
                // anything you like to do with target failure 
                // you have access to deserialized TargetErrorModel from ex.ErrorModel

                throw;
            }
        }
    }
```

## More Features

#### Creating request with query parameters:
```csharp
    var parameters = new Dictionary<string, string>
    {
        ["param1"] = "ABCD",
        ["param2"] = "2",
        ["param3"] = "true",
    };
    var relativeUri = new Uri("service/address", UriKind.Relative);
    var uri = MinimalHttpHelpers.GenerateQueryParamUri(httpClient.BaseAddress, relativeUri, parameters);
    
    //Result: <BaseAddress>/service/address?param1=ABCD&param2=2&param3=true

```

#### Creating request with Json body:

```csharp
    var model = new 
    {
        param1 = "ABCD",
        param2 = 2,
        param3 = true
    };

    var httpRequest = new HttpRequestMessage();

    // Adds JsonContent to body of request
    httpRequest.AddJsonContent(model);
```

#### More complex scenarios (AddJsonContent vs AddJsonContentFreeStyle):

```csharp
    public class ParentType
    {
        public string A { get; set; }
    }

    public class ChildType : ParentType
    {
        public string B { get; set; }
    }

    public Task MethodA(ParentType model)
    {
        var httpRequest = new HttpRequestMessage();

        // lets assume passed model to this method is a 'ChildType' which is also a 'ParentType' 

        // As model is ParentType serialized model will have just parameter 'A'
        httpRequest.AddJsonContent(model);

        // As model is actually a 'ChildType' serialized model will have both parameters 'A' and 'B'
        httpRequest.AddJsonContentFreeStyle(model);
    }
```