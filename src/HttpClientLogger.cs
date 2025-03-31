using Microsoft.Extensions.Logging;

namespace MinimalHttp;

public partial class HttpClientLogger(
    HttpMessageHandler innerHandler,
    HttpClientLoggerOptions loggerOptions,
    ILogger<HttpClientLogger> logger)
    : DelegatingHandler(innerHandler)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var requestBody = string.Empty;
        var responseBody = string.Empty;
        HttpResponseMessage? response = null;

        try
        {
            if (loggerOptions.RequestBody && request.Content != null)
            {
                requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            }

            response = await base.SendAsync(request, cancellationToken);

            if (loggerOptions.ResponseBody && response.Content != null)
            {
                responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            }

            return response;
        }
        finally
        {
            LogHttpCall(logger, request.RequestUri, requestBody, (int?)response?.StatusCode, response?.ReasonPhrase, responseBody);
        }
    }
    
    [LoggerMessage(
        Message = "Calling Http request '{requestUri}' with body : '{requestBody}'. Getting status code '{statusCode}', ReasonPhrase: '{reasonPhrase}' with response body '{responseBody}'",
        Level = LogLevel.Information,
        EventId = 1,
        EventName = "HttpCall")]
    public static partial void LogHttpCall(ILogger logger, Uri? requestUri, string requestBody, int? statusCode, string? reasonPhrase, string responseBody);
}

