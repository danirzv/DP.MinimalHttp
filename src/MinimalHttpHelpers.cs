using System.Net.Http.Json;
using System.Web;

namespace MinimalHttp;

/// <summary>
/// Contains Helpers which could help to handel httpMessage needs
/// </summary>
public static class MinimalHttpHelpers
{
    /// <summary>
    /// Serializing input and add as JsonContent to request body 
    /// </summary>
    /// <param name="request">HttpRequestMessage</param>
    /// <param name="input">Serializing object</param>
    /// <typeparam name="T">Type of serializing object</typeparam>
    /// <returns></returns>
    public static HttpRequestMessage AddJsonContent<T>(this HttpRequestMessage request, T input)
    {
        request.Content = JsonContent.Create(input);

        return request;
    }

    /// <summary>
    /// Serializing input and add as JsonContent to request body as any type it is in runtime
    /// </summary>
    /// <param name="request">HttpRequestMessage</param>
    /// <param name="input">Serializing object</param>
    /// <returns></returns>
    public static HttpRequestMessage AddJsonContentFreeStyle(this HttpRequestMessage request, object input)
    {
        request.Content = JsonContent.Create(input, input.GetType());

        return request;
    }

    /// <summary>
    /// Creates a Uri with desired query parameters
    /// </summary>
    /// <param name="baseUri">base of requesting endpoint Uri</param>
    /// <param name="relative">relative part of requesting endpoint Uri</param>
    /// <param name="parameters">query parameters, if sending array in query param send elements as arrayParam[0], arrayParam[1], ...</param>
    /// <returns>Uri containing query parameters</returns>
    public static Uri GenerateQueryParamUri(Uri baseUri, Uri relative, IReadOnlyDictionary<string, string> parameters)
    {
        var absoluteUri = new Uri(baseUri, relative);
        return GenerateQueryParamUri(absoluteUri, parameters);
    }
    
    /// <summary>
    /// Creates a Uri with desired query parameters  
    /// </summary>
    /// <param name="absoluteUri">absolute address of requesting endpoint Uri</param>
    /// <param name="parameters">query parameters, if sending array in query param send elements as arrayParam[0], arrayParam[1], ...</param>
    /// <returns>Uri containing query parameters</returns>
    public static Uri GenerateQueryParamUri(Uri absoluteUri, IReadOnlyDictionary<string, string> parameters)
    {
        var uriBuilder = new UriBuilder(absoluteUri);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var param in parameters)
        {
            queryParams.Add(param.Key, param.Value);
        }

        uriBuilder.Query = queryParams.ToString();

        return uriBuilder.Uri;
    }
}
