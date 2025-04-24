using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;

namespace MinimalHttp;

public static class HttpClientExtensions
{
    /// <summary>
    /// you may set it as you desire
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonSerializerOptions.Default;

    /// <summary>
    /// Makes a http call and returns response
    /// In case of unsuccessful request tries to deserialize TErrorModel and throw an <see cref="ExternalProviderException{TErrorModel}"/>
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="request">HttpRequestMessage</param>
    /// <param name="logger">ILogger</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <param name="customSuccessfulResultIndicator">Function which can define a request was successful regardless of statusCode 2xx</param>
    /// <typeparam name="TResponse">Successful response model</typeparam>
    /// <typeparam name="TErrorModel">Unsuccessful response model</typeparam>
    /// <exception cref="ExternalProviderException{TErrorModel}">in case of unsuccessful response</exception>
    /// <exception cref="Exception">in case of fail in deserialize</exception>
    public static async Task<TResponse> SendAsync<TResponse, TErrorModel>(this HttpClient client, HttpRequestMessage request, ILogger logger, CancellationToken cancellationToken = default,
        Func<TResponse, HttpResponseMessage, bool>? customSuccessfulResultIndicator = null, JsonSerializerOptions? customJsonSerializerOptions = null)
        where TResponse : class
        where TErrorModel : IExternalProviderError
    {
        using var result = await client.SendAsync(request, cancellationToken);

        var resultContentStream = await result.Content.ReadAsStreamAsync(cancellationToken);

        if (!result.IsSuccessStatusCode)
        {
            logger.LogWarning("API call failed, HandlingError");
            throw await GenerateException<TErrorModel>(resultContentStream, result.StatusCode, customJsonSerializerOptions);
        }

        var response = await JsonSerializer.DeserializeAsync<TResponse>(resultContentStream, customJsonSerializerOptions ?? JsonSerializerOptions, cancellationToken);

        if (response is null || (customSuccessfulResultIndicator != null && !customSuccessfulResultIndicator(response, result)))
        {
            logger.LogWarning("API call failed, HandlingError");
            resultContentStream.Position = 0;
            throw await GenerateException<TErrorModel>(resultContentStream, result.StatusCode, customJsonSerializerOptions);
        }

        return response;
    }

    /// <summary>
    /// Makes a http call and returns response
    /// In case of unsuccessful request tries to deserialize TErrorModel and throw an <see cref="ExternalProviderException{TErrorModel}"/>
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="request">HttpRequestMessage</param>
    /// <param name="logger">ILogger</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <param name="customSuccessfulResultIndicator">Function which can define a request was successful regardless of statusCode 2xx</param>
    /// <typeparam name="TErrorModel">Unsuccessful response model</typeparam>
    /// <exception cref="ExternalProviderException{TErrorModel}">in case of unsuccessful response</exception>
    /// <exception cref="Exception">in case of fail in deserialize</exception>
    public static async Task SendAsync<TErrorModel>(this HttpClient client, HttpRequestMessage request, ILogger logger, CancellationToken cancellationToken = default,
        Func<HttpResponseMessage, bool>? customSuccessfulResultIndicator = null, JsonSerializerOptions? customJsonSerializerOptions = null)
        where TErrorModel : IExternalProviderError
    {
        using var result = await client.SendAsync(request, cancellationToken);

        var resultContentStream = await result.Content.ReadAsStreamAsync(cancellationToken);

        if (!result.IsSuccessStatusCode || (customSuccessfulResultIndicator != null && customSuccessfulResultIndicator(result)))
        {
            logger.LogWarning("API call failed, HandlingError");
            throw await GenerateException<TErrorModel>(resultContentStream, result.StatusCode, customJsonSerializerOptions);
        }
    }

    private static async Task<Exception> GenerateException<TErrorModel>(Stream resultStream, HttpStatusCode statusCode, JsonSerializerOptions? customJsonSerializerOptions = null)
        where TErrorModel : IExternalProviderError
    {
        var error = await JsonSerializer.DeserializeAsync<TErrorModel>(resultStream, customJsonSerializerOptions ?? JsonSerializerOptions);
        return new ExternalProviderException<TErrorModel>(
            $"{error?.GetCode() ?? "Unknown"}",
            error?.GetTitle() ?? string.Empty,
            error?.GetMessage() ?? string.Empty,
            statusCode,
            error?.GetErrors() ?? new Dictionary<string, string[]>(),
            error);
    }
}