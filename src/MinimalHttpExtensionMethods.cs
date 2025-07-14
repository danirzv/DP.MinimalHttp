using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalHttp;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains methods to setup functionalities
/// </summary>
public static class MinimalHttpExtensionMethods
{
    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your HttpClient</typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>()
            .SetupDefaultFunctionality<TOptions>();
    }

    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClientHandler">which will be used to configure your <see cref="T:System.Net.Http.HttpClientHandler" /></param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your <see cref="T:System.Net.Http.HttpClient" /></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services, Action<HttpClientHandler> configureClientHandler)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>()
            .SetupDefaultFunctionality<TOptions>(configureClientHandler);
    }

    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="T:System.Net.Http.HttpClient" />.</param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your <see cref="T:System.Net.Http.HttpClient" /></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services, Action<HttpClient> configureClient)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>(configureClient)
            .SetupDefaultFunctionality<TOptions>();
    }

    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="T:System.Net.Http.HttpClient" />.</param>
    /// <param name="configureClientHandler">which will be used to configure your <see cref="T:System.Net.Http.HttpClientHandler" /></param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your <see cref="T:System.Net.Http.HttpClient" /></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services, Action<HttpClient> configureClient, Action<HttpClientHandler> configureClientHandler)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>(configureClient)
            .SetupDefaultFunctionality<TOptions>(configureClientHandler);
    }
    
    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="T:System.Net.Http.HttpClient" />.</param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your <see cref="T:System.Net.Http.HttpClient" /></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>(configureClient)
            .SetupDefaultFunctionality<TOptions>();
    }

    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="T:System.Net.Http.HttpClient" />.</param>
    /// <param name="configureClientHandler">which will be used to configure your <see cref="T:System.Net.Http.HttpClientHandler" /></param>
    /// <typeparam name="TClient">The class which named <see cref="T:System.Net.Http.HttpClient" /> will be assigned to, make sure to register it before this method call</typeparam>
    /// <typeparam name="TOptions">The <see cref="MinimalHttp.HttpClientOptions"/> which will be used to configure your <see cref="T:System.Net.Http.HttpClient" /></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<HttpClientHandler> configureClientHandler)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>(configureClient)
            .SetupDefaultFunctionality<TOptions>(configureClientHandler);
    }

    private static IHttpClientBuilder SetupDefaultFunctionality<TOptions>(this IHttpClientBuilder builder, Action<HttpClientHandler>? configureClientHandler = null)
        where TOptions : HttpClientOptions
    {
        return builder.ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetService<IOptionsMonitor<TOptions>>()!.CurrentValue;

                httpClient.BaseAddress = options.BaseUri;
                httpClient.Timeout = options.Timeout;
            })
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var options = sp.GetService<IOptionsMonitor<TOptions>>()!.CurrentValue;

                var realHandler = new HttpClientHandler();

                if (configureClientHandler is not null)
                {
                    configureClientHandler.Invoke(realHandler);
                }
                else
                {
                    realHandler.UseProxy = false;
                    realHandler.AllowAutoRedirect = false;
                    realHandler.UseCookies = false;
                }

                realHandler.ServerCertificateCustomValidationCallback = options.CertificateValidationCallback!;

                var loggingWrapper = new HttpClientLogger(
                    realHandler,
                    new HttpClientLoggerOptions
                    {
                        RequestBody = options.HttpClientLoggingMode.HasFlag(HttpClientLoggingMode.RequestBody),
                        ResponseBody = options.HttpClientLoggingMode.HasFlag(HttpClientLoggingMode.ResponseBody),
                    },
                    sp.GetService<ILogger<HttpClientLogger>>()!);

                return loggingWrapper;
            });
    }
}
