using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalHttp;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalHttpExtensionMethods
{
    /// <summary>
    /// Registers <see cref="T:System.Net.Http.HttpClient" /> with options presented in TOptions and name of TClient type
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpClient<TClient, TOptions>(this IServiceCollection services)
        where TClient : class
        where TOptions : HttpClientOptions
    {
        return services.AddHttpClient<TClient>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetService<IOptionsMonitor<TOptions>>()!.CurrentValue;

                httpClient.BaseAddress = options.BaseUri;
                httpClient.Timeout = options.Timeout;
            })
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var options = sp.GetService<IOptionsMonitor<TOptions>>()!.CurrentValue;

                var realHandler = new HttpClientHandler();

                realHandler.UseProxy = false;
                realHandler.AllowAutoRedirect = false;
                realHandler.UseCookies = false;
                realHandler.ServerCertificateCustomValidationCallback = options.CertificateValidationCallback;

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
